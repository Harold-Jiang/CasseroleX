using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Queries;
public class GetRoleTreeQuery : IRequest<List<RoleTreeDto>>
{
    public int Id { get; set; }
    public int Pid { get; set; }
} 

public class GetRoleTreeHandler : IRequestHandler<GetRoleTreeQuery, List<RoleTreeDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;

    public GetRoleTreeHandler(IApplicationDbContext context,
        IMapper mapper,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<List<RoleTreeDto>> Handle(GetRoleTreeQuery request, CancellationToken cancellationToken)
    {
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);

        var parentGroupModel = await _context.Roles.FindAsync(new object?[] { request.Pid }, cancellationToken: cancellationToken);
        Role? currentGroupModel = null;
        if (request.Id > 0)
        {
            currentGroupModel = await _context.Roles.FindAsync(new object?[] { request.Id }, cancellationToken: cancellationToken);
        }
        if ((request.Pid > 0 || parentGroupModel is not null)
            && (request.Id == 0 || currentGroupModel is not null))
        {
            var ruleList = await _context.RolePermissions
                .OrderByDescending(x => x.Weigh)
                .OrderBy(x => x.Id)
                .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            // Read the list of all nodes in the parent role
            var parentRuleList = new List<MenuDto>();
            if (parentGroupModel!.Rules.Split(',').Contains("*"))
            {
                parentRuleList = ruleList;
            }
            else
            {
                var parentRuleIds = parentGroupModel!.Rules.ToIList<int>();
                foreach (var rule in ruleList)
                {
                    if (parentRuleIds.Contains(rule.Id))
                    {
                        parentRuleList.Add(rule);
                    }
                }
            }
         
            var ruleTree = parentRuleList;
            var groupTree = await _context.Roles
                                    .Where(x => childrenRoleIds.Contains(x.Id))
                                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync(cancellationToken: cancellationToken);

            var adminRuleIds = _currentUserService.PermissionIds;
            var superadmin = _currentUserService.IsSuperAdmin;

            var currentRuleIds = currentGroupModel?.Rules.ToIList<int>() ?? new();

            if (request.Id == 0 || !childrenRoleIds.Contains(request.Pid) || !Tree.GetChildrenIds(groupTree, request.Id, true).Contains(request.Pid))
            {
                parentRuleList = Tree.GetTreeList(Tree.GetTreeArray(ruleTree, 0));
                var hasChildrens = parentRuleList.Where(v => v.HasChild == 1).Select(v => v.Id).ToList();
                var parentRuleIds = parentRuleList.Select(item => item.Pid).ToList();
                List<RoleTreeDto> nodeList = new();

                foreach (var rule in parentRuleList)
                {
                    if (!superadmin && !adminRuleIds.Contains(rule.Pid.ToString()))
                    {
                        continue;
                    }

                    if (rule.Pid > 0 && !parentRuleIds.Contains(rule.Pid))
                    {
                        continue;
                    }

                    var state = new Dictionary<string, bool>
                    {
                        { "selected", currentRuleIds.Contains(rule.Id) && !hasChildrens.Contains(rule.Id) }
                    };

                    nodeList.Add(new RoleTreeDto
                    {
                        Id = rule.Id,
                        Parent = rule.Pid > 0 ? rule.Pid.ToString() : "#",
                        Text = rule.Title,
                        State = state
                    });

                }

                return nodeList;
            }
            else
            {
                throw new ArgumentException("Cannot change parent to child");
            }
        }
        else
        {
            throw new NotFoundException(nameof(Role), request.Id);
        }
    }

}
