using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Queries;
public class GetRoleGroupsQuery: IRequest<List<RolesDto>>
{
    public int UserId { get;set; }
    public bool IsSuperAdmin { get; set; }
}

public class GetRoleGroupsHandler : IRequestHandler<GetRoleGroupsQuery, List<RolesDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;
    public GetRoleGroupsHandler(IApplicationDbContext context,
        IMapper mapper,
        IRoleManager roleManager)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<List<RolesDto>> Handle(GetRoleGroupsQuery request, CancellationToken cancellationToken)
    {
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(request.UserId, true, cancellationToken);
        return await GetCurrentRoles(childrenRoleIds, request.IsSuperAdmin);
    }

    public async Task<List<RolesDto>> GetCurrentRoles(List<int> childrenRoleIds,bool isSuperAdmin)
    {
        //get all role group
        var roleList = await _context.Roles
            .Where(x => childrenRoleIds.Contains(x.Id))
            .ProjectTo<RolesDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        List<RolesDto> currentRoleList = new();
        if (isSuperAdmin)
        {
            currentRoleList = Tree.GetTreeList(Tree.GetTreeArray(roleList, 0));
        }
        else
        {
            var roles = await _context.AdminRoles
                .Include(x => x.Role)
                .Select(s => s.Role)
                .ProjectTo<RolesDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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
