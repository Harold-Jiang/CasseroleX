using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Commands.CreateRole;
public class CreateRoleCommand : IRequest<Result>
{ 
    public int Pid { get; init; }
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; }

}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateRoleCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;

    public CreateTodoItemCommandHandler(IApplicationDbContext context,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
      
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        if (!childrenRoleIds.Contains(request.Pid))
        {
            throw new PermissionException("父角色组超出权限限制");
        }
        var parentModel = await _context.Roles
            .FirstOrDefaultAsync(x=>x.Id == request.Pid,cancellationToken) ?? throw new PermissionException("找不到角色组");

        var rules = request.Rules.ToIList<string>();
        var parentRules = parentModel.Rules.ToIList<string>();
        var currentRules = _currentUserService.PermissionIds;

        rules = parentRules.Contains("*") ? rules : rules.Intersect(parentRules).ToList();
        rules = currentRules.Contains("*") ? rules : rules.Intersect(currentRules).ToList();

        var entity = new Role
        {
            Rules = string.Join(",", rules),
            Name = request.Name,
            Status = request.Status,
            Pid = request.Pid
        };

        await _context.Roles.AddAsync(entity, cancellationToken);
        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success(entity.Id) : Result.Failure();
    }
}
