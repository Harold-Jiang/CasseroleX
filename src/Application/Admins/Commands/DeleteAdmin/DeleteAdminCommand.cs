using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Commands.DeleteAdmin;
public record DeleteAdminCommand(string Ids) : IRequest<Result>;

public class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAdminCommandHandler(IApplicationDbContext context,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

        var childrenAdminIds = await _roleManager.GetChildrenAdminIds(_currentUserService.IsSuperAdmin, _currentUserService.UserId, true, cancellationToken);

        var idsArray = idList.Where(childrenAdminIds.Contains).ToArray();

        // Avoiding unauthorized deletion of administrators
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);

        await _context.AdminRoles
                    .Include(x => x.Admin)
                    .Where(x => idsArray.Contains(x.Admin.Id)
                        && childrenRoleIds.Contains(x.RoleId))
                    .Select(x => x.Admin)
                    .ExecuteDeleteAsync(cancellationToken);

        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success() : Result.Failure();

    }
}
