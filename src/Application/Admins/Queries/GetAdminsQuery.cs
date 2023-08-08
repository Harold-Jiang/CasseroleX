using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Queries;

public class GetAdminsQueryHandler : IRequestHandler<SearchQuery<Admin,AdminDto>, PaginatedList<AdminDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    public GetAdminsQueryHandler(IApplicationDbContext context,
        IMapper mapper,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<AdminDto>> Handle(SearchQuery<Admin, AdminDto> request, CancellationToken cancellationToken)
    { 
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        var childrenAdminIds = await _roleManager.GetChildrenAdminIds(_currentUserService.IsSuperAdmin, _currentUserService.UserId, true, cancellationToken);
         
        
        var roles = await _context.AdminRoles
            .Include(x=>x.Role)
            .Where(a => childrenRoleIds.Contains(a.RoleId)) 
            .ToListAsync(cancellationToken);

       // var roleName = roles
          // .ToLookup(g => g.Role.Id, g => g.Role.Name);

        var adminGroupName = roles
                .GroupBy(r => r.AdminId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(r => r.RoleId, r => r.Role.Name ?? "")
                );

        var curRoles = await _roleManager.GetRolesAsync(_currentUserService.UserId, cancellationToken);

        adminGroupName[_currentUserService.UserId] = curRoles
            .ToDictionary(role => role.Id, role => role.Name);

        //var adminGroupName = new Dictionary<int, Dictionary<int, string>>();
        //foreach (var role in roles)
        //{
        //    if (roleName.TryGetValue(role.RoleId, out string? name))
        //    {
        //        if (!adminGroupName.ContainsKey(role.AdminId))
        //        {
        //            adminGroupName[role.AdminId] = new Dictionary<int, string>();
        //        }
        //        adminGroupName[role.AdminId][role.RoleId] = name ?? "";
        //    }
        //}

        //var curRoles = await _roleManager.GetRolesAsync(_currentUserService.UserId, cancellationToken);
        //foreach (var role in curRoles)
        //{
        //    if (!adminGroupName.ContainsKey(role.Id))
        //    {
        //        adminGroupName[role.Id] = new Dictionary<int, string>();
        //    }
        //    adminGroupName[_currentUserService.UserId][role.Id] = role.Name;
        //}


        var admins = await _context.Admins
            .Where(a => childrenAdminIds.Contains(a.Id)) 
            .Where(request.GetQueryLamda())
            .ProjectTo<AdminDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);

        foreach (var item in admins.Items)
        {
            if (adminGroupName.TryGetValue(item.Id, out var groups))
            {
                item.RoleIds = string.Join(",", groups.Keys);
                item.GroupsText = string.Join(",", groups.Values);
            }
        }

        return admins;

    }
}
