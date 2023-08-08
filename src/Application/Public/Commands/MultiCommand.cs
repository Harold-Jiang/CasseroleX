using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Common;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.Public.Commands;

public class MultiCommand<T> : IRequest<Result> where T : BaseEntity
{
    public required string Ids { get; set; } 
    public required string Params { get; set; }   
    public string? Action { get; set; }
}
public class MultiCommandHandler<T> : IRequestHandler<MultiCommand<T>, Result> where T : BaseEntity
{
    private readonly IApplicationDbContext _context;
    private static readonly string[] multiFields = new string[] { "IsMenu","Status" };

    public MultiCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(MultiCommand<T> request, CancellationToken cancellationToken)
    {
        var ids = request.Ids.ToIList<int>();
        if (ids.Count == 0)
            throw new ArgumentException(nameof(request.Ids));

        string[] arr = request.Params.Split("=");
        var propertyName = arr[0].GetPropertyName<T>();
        if (string.IsNullOrEmpty(propertyName) || !multiFields.Contains(propertyName))
            throw new ArgumentException(propertyName);

        foreach ( var id in ids)
        {
            var model = await _context.Set<T>().FindAsync(new object?[] { id }, cancellationToken);
            var propertyInfo =  model?.GetType().GetProperty(propertyName);
            if (model != null && propertyInfo != null)
            {
                if (propertyName == "IsMenu")
                    model.SetPropertyValue(propertyName, arr[1].ToBool());
                else
                    model.SetPropertyValue(propertyName, arr[1].ToEnum<Status>());
                _context.Set<T>().Entry(model).Property(propertyName).IsModified =  true;
            }
        }
       
        return await _context.SaveChangesAsync(cancellationToken) > 0 ? Result.Success() : Result.Failure();
    }
}
