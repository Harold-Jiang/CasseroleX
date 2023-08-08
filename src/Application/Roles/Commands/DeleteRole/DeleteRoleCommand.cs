using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Commands.DeleteRole;
public class DeleteRoleCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

        var roleIds = _currentUserService.RoleIds;
        var idsArray = idList.Except(roleIds).ToList();

        var roleList = await _context.Roles
            .Where(x => idsArray.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var role in roleList)
        {
            // There are administrators in the current role
            var adminRole = await _context.AdminRoles.FirstOrDefaultAsync(x => x.RoleId == role.Id, cancellationToken);
            if (adminRole is not null)
            {
                idsArray.Remove(role.Id);
                continue;
            }
            // There are sub characters under the current character
            var childrenRole = await _context.Roles.FirstOrDefaultAsync(x => x.Pid == role.Id, cancellationToken);
            if (childrenRole is not null)
            {
                idsArray.Remove(role.Id);
                continue;
            }
        }
        if (idsArray.Count == 0)
        {
            throw new PermissionException("Role groups containing sub roles and administrator roles cannot be deleted");
        }

        var result = await _context.Roles
            .Where(x => idsArray.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken) > 0;

        return result ? Result.Success() : Result.Failure();

    }
}
