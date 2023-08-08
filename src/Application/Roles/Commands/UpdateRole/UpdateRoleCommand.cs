using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Roles.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Commands.UpdateRole;
public class UpdateRoleCommand :  IRequest<Result>
{
    public int Id { get; set; } 
    public int Pid { get; init; }
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;


    public UpdateRoleCommandHandler(IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IRoleManager roleManager,
        IMapper mapper)
    {
        _context = context;
        _currentUserService = currentUserService;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles
            .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(Role), request.Id);

        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        if (!childrenRoleIds.Contains(request.Pid))
        {
            throw new PermissionException("Parent role group exceeds permission limit");
        }
        var roleList = await _context.Roles
           .Where(x => childrenRoleIds.Contains(x.Id))
           .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
           .ToListAsync(cancellationToken: cancellationToken);

        if (Tree.GetChildrenIds(roleList,request.Id, true).Contains(request.Pid)) {
            throw new PermissionException("The parent node cannot be its own child node or itself");
        }

        var parentModel = await _context.Roles
              .FirstOrDefaultAsync(x => x.Id == request.Pid, cancellationToken) ?? throw new NotFoundException(nameof(Role),request.Id);

        var rules = request.Rules.ToIList<string>();
        var parentRules = parentModel.Rules.ToIList<string>();
        var currentRules = _currentUserService.PermissionIds;

        rules = parentRules.Contains("*") ? rules : rules.Intersect(parentRules).ToList();
        rules = currentRules.Contains("*") ? rules : rules.Intersect(currentRules).ToList();


        entity.Rules = string.Join(",", rules);
        entity.Name = request.Name;
        entity.Pid = request.Pid;
        entity.Status = request.Status;

        _context.Roles.Update(entity);

        var ids = Tree.GetChildrenIds(roleList, request.Id);

        var childrenRoles = await _context.Roles
                                     .Where(x => ids.Contains(x.Id))
                                     .ToListAsync(cancellationToken);

        var childParams = childrenRoles.Select(childrenRole => new Role
        {
            Id = childrenRole.Id,
            Rules = string.Join(",", childrenRole.Rules.Split(',').Intersect(rules))
        }).ToList();

        _context.Roles.UpdateRange(childParams);
         
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success() : Result.Failure();
    } 
}
