using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Commands.UpdateAdmin;
public record UpdateAdminCommand : IRequest<Result>
{
    public int Id { get; set; }
    public required int[] RoleIds { get; init; }

    public string? Password { get; init; }

    public required string NickName { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Mobile { get; set; }

    public int LoginFailure { get; set; }
}

public class UpdateAdminCommandHandler : IRequestHandler<UpdateAdminCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly IRoleManager _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAdminCommandHandler(IApplicationDbContext context,
        IEncryptionService encryptionService,
        ICurrentUserService currentUserService,
        IRoleManager roleManager)
    {
        _context = context;
        _encryptionService = encryptionService;
        _currentUserService = currentUserService;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(UpdateAdminCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins
            .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(Admin), request.Id);

        var childrenAdminIds = await _roleManager.GetChildrenAdminIds(_currentUserService.IsSuperAdmin, _currentUserService.UserId, true, cancellationToken);

        if (!childrenAdminIds.Contains(request.Id))
        {
            throw new ForbiddenAccessException();
        }

        //edit password
        if (request.Password.IsNotNullOrEmpty()) 
        {
            var salt = _encryptionService.CreateSaltKey(6);
            entity.PasswordHash = _encryptionService.CreatePasswordHash(request.Password, salt);
        }
       
        entity.UserName = request.UserName;
        entity.NickName = request.NickName;
        entity.Email = request.Email;
        entity.Mobile = request.Mobile;
        entity.LoginFailure = request.LoginFailure;

        _context.Admins.Update(entity);

        // Remove all permissions first
        await _context.AdminRoles.Where(a => a.AdminId == request.Id).ExecuteDeleteAsync(cancellationToken);

        var rolesIds = await CheckRolesAuth(request.RoleIds.ToList(), cancellationToken); 

        var adminRoleList = rolesIds.Select(id => new AdminRole
        {
            AdminId = request.Id,
            RoleId = id

        }).ToList();

        _context.AdminRoles.AddRange(adminRoleList);

        return (await _context.SaveChangesAsync(cancellationToken) > 0) ? Result.Success() : Result.Failure();

    }

    // Filter out groups that are not allowed to exceed authority
    private async Task<List<int>> CheckRolesAuth(List<int> rolesIds, CancellationToken cancellationToken = default)
    { 
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        rolesIds = rolesIds.Intersect(childrenRoleIds).ToList();
        if (rolesIds.Count == 0)
        {
            throw new Exception("The parent group exceeds permission limit");
        }
        return rolesIds;
    }

}
