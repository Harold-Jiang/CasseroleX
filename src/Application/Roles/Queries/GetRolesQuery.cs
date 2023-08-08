using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Queries;
public record GetRolesQuery : IRequest<List<RoleDto>>;

public class GetRolesHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;

    public GetRolesHandler(IApplicationDbContext context,
        IMapper mapper,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        return await GetCurrentRoles(childrenRoleIds, _currentUserService.IsSuperAdmin, cancellationToken);
    }

    public async Task<List<RoleDto>> GetCurrentRoles(List<int> childrenRoleIds, bool isSuperAdmin, CancellationToken cancellationToken)
    {
        //get all role group
        var roleList = await _context.Roles
            .Where(x => childrenRoleIds.Contains(x.Id))
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        List<RoleDto> currentRoleList = new();
        if (isSuperAdmin)
        {
            currentRoleList = Tree.GetTreeList(Tree.GetTreeArray(roleList, 0));
        }
        else
        {
            var roles = await _roleManager.GetRolesAsync(_currentUserService.UserId, cancellationToken);
            var roleIds = new List<int>();
            foreach (var role in roles)
            {
                if (roleIds.Contains(role.Id) || roleIds.Contains(role.Pid))
                {
                    continue;
                }
                currentRoleList.AddRange(Tree.GetTreeList(Tree.GetTreeArray(roleList, role.Pid)));
                foreach (var item in currentRoleList)
                {
                    roleIds.Add(item.Id);
                }
            }
        }
        return currentRoleList;
    }
}
