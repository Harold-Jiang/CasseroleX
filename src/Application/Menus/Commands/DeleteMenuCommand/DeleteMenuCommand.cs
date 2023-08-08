using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Commands.DeleteMenuCommand;
public class DeleteMenuCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public DeleteMenuCommandHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

        foreach (var id in idList.ToList())
        {
            var rolePermissions = await _context.RolePermissions
               .Select(s => new RolePermissions { Id = s.Id, Pid = s.Pid })
               .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);

            var childrenIds = Tree.GetChildrenIds(rolePermissions, id, true);
            idList.AddRange(childrenIds);
        }
        idList = idList.Distinct().ToList();

        var count = await _context.RolePermissions
                    .Where(x => idList.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken); 

        var admins = await _context.Admins.Select(x => x.Id).ToListAsync(cancellationToken);
        foreach (var admin in admins)
        {
            //remove cache
            await _cache.RemoveAsync(string.Format(CacheKeys.ADMIN_ROLEPERMISSIONS_BY_ADMINID_KEY, admin), cancellationToken);
        }
        return count > 0 ? Result.Success() : Result.Failure();

    }
}
