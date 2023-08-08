using System.Linq.Expressions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Public.Commands;
public class SortCommand<T> : IRequest<Result> where T : BaseEntity
{
    public required string Ids { get; set; }
    public int ChangeId { get; set; }
    public int Pid { get; set; }
    public string? Field { get; set; } 
    public required string Orderway { get; set; } 
    public required string Table { get; set; }
    public required string Pk { get; set; }
     
}

public class SortCommandHandler<T> : IRequestHandler<SortCommand<T>, Result> where T : BaseEntity
{
    private readonly IApplicationDbContext _context;

    public SortCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(SortCommand<T> request, CancellationToken cancellationToken)
    {
        List<int> idsList = request.Ids.ToIList<int>();
        request.Field = request.Field.GetPropertyName<T>();
        if (idsList.Count == 0 || !request.Field.IsNotNullOrEmpty())
        {
            throw new ArgumentException(nameof(request.Table));
        }
        var orderway = request.Orderway == "asc" ? "ASC" : "DESC";
        List<int> sour = new();
        Dictionary<int, int> weighdata = new();
        IEnumerable<T> list = new List<T>();

    
        // If pid is specified, only match IDs that meet the condition and ignore others
        if (request.Pid != 0)
        { 
            var hasids = await _context.Set<T>()
                .Where(SortCommandHandler<T>.GetExpression(idsList, request.Pid))
                .Select(o => o.Id)
                .ToListAsync(cancellationToken);

            idsList = idsList.Intersect(hasids).ToList();

        }

        if (orderway == "ASC")
        {
            list = await _context.Set<T>()
                .OrderBy(GetOrderBy(request.Field))
                .Where(x => idsList.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
        else
        {
            list = await _context.Set<T>()
                .OrderByDescending(GetOrderBy(request.Field))
                .Where(x => idsList.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }

        //Sorting of existing menu IDs
        foreach (var item in list)
        {
            var weigh = item.GetType().GetProperty(request.Field);
            sour.Add(item.Id);
            weighdata.Add(item.Id, weigh == null ? 0 : weigh.GetValue(item).ToInt());
        }

        int position = idsList.IndexOf(request.ChangeId);
        int desc_id = sour.ElementAtOrDefault(position) != 0 ? sour.ElementAtOrDefault(position) : sour.LastOrDefault();
        // Move to the target ID value, retrieve the value of the previous position before the change
        int sour_id = request.ChangeId;
        var weighids = new Dictionary<int, int>();
        //var temp = idsList.Except(sour).ToList();
        List<int> temp = new();
        for (int i = 0; i < sour.Count; i++)
        {
            if (idsList[i] != sour[i])
            {
                temp.Add(idsList[i]);
            }
        }
        for (int i = 0; i < temp.Count; i++)
        {
            int n = temp[i];
            int offset;
            if (n == sour_id)
            {
                offset = desc_id;
            }
            else
            {
                offset = sour_id == temp.First() ? (temp.ElementAtOrDefault(i + 1) != 0 ? temp.ElementAtOrDefault(i + 1) : sour_id) : (temp.ElementAtOrDefault(i - 1) != 0 ? temp.ElementAtOrDefault(i - 1) : sour_id);
            }

            if (!weighdata.ContainsKey(offset))
            {
                continue;
            }

            weighids[n] = weighdata[offset];
            var entity = _context.Set<T>().Find(n);
            if (entity is not null)
            {
                entity.GetType().GetProperty(request.Field)?.SetValue(entity, weighdata[offset]);
                _context.Set<T>().Entry(entity).State = EntityState.Modified;
            }
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0 ? Result.Success() : Result.Failure();
    }


    private static Expression<Func<T, bool>> GetExpression(List<int> ids,int pid)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, "Id");
        var containsMethod = typeof(List<int>).GetMethod("Contains");
        var idsConstant = Expression.Constant(ids);
        var containsCall = Expression.Call(idsConstant, containsMethod, property);

        var propertyPid = Expression.Property(parameter, "Pid");
        var requestPidConstant = Expression.Constant(pid);
        var pidEquals = Expression.Equal(propertyPid, requestPidConstant);

        var andAlso = Expression.AndAlso(containsCall, pidEquals);

        return Expression.Lambda<Func<T, bool>>(andAlso, parameter);
    } 

    private static Expression<Func<T,int>> GetOrderBy(string field)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, field);
        return Expression.Lambda<Func<T, int>>(propertyAccess, parameter);
    }
}
