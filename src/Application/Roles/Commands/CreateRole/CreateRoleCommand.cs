using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using CasseroleX.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Commands.CreateRole;
public record CreateRoleCommand : IRequest<int>
{
    public int UserId { get; set; }
    public int Pid { get; init; }
    public string Rules { get; init; } = null!;
    public string Name { get; set; } = null!;
    public Status Status { get; set; }

}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;

    public CreateTodoItemCommandHandler(IApplicationDbContext context, 
        IRoleManager roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var rules  = request.Rules.ToIList<string>();
        var childrenGroupIds = await _roleManager.GetChildrenRoleIds(request.UserId, true);
        if (!childrenGroupIds.Contains(request.Pid))
        {
            throw new JsonResultException("The parent group exceeds permission limit");
        }
        var parentModel = await _context.Roles
            .FirstOrDefaultAsync(x=>x.Id == request.Pid,cancellationToken) ?? throw new JsonResultException("The parent group can not be found");

        var parentRules = parentModel.Rules.Split(',').ToList();
        var (currentRules,roleIds)= await _roleManager.GetRolePermissionIdsAsync(request.UserId,cancellationToken);

        rules = parentRules.Contains("*") ? rules : rules.Intersect(parentRules).ToList();
        rules = currentRules.Contains("*") ? rules : rules.Intersect(currentRules).ToList();

        var entity = new Role
        {
            Rules = string.Join(",", rules),
            Name = request.Name,
            Status = request.Status,
            Pid = request.Pid
        };

        //entity.AddDomainEvent(new RoleCreatedEvent(entity));

        await _context.Roles.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
