using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.Admins.Commands.CreateAdmin;
public class CreateAdminCommand : IRequest<Result>
{
    public required int[] RoleIds { get; init; }

    public required string Password { get; init; }

    public required string NickName { get; set; }

    public required string UserName { get; set; } 

    public required string Email { get; set; }

    public required string Mobile { get; set; }

    public required Status Status { get; set; }

}

public class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly IRoleManager _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public CreateAdminCommandHandler(IApplicationDbContext context,
        IEncryptionService encryptionService,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var salt = _encryptionService.CreateSaltKey(6);
        var admin = new Admin
        {
            Salt = salt,
            PasswordHash = _encryptionService.CreatePasswordHash(request.Password, salt),
            Avatar = "/assets/img/avatar.png",
            UserName = request.UserName,
            NickName = request.NickName,
            Email = request.Email,
            Mobile = request.Mobile,
            Status = request.Status
        };
        using var transaction = _context.BeginTransactionAsync(cancellationToken);
        try
        {
            // save admin 
            await _context.Admins.AddAsync(admin, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // save adminRole 
            var rolesIds = await CheckRolesAuth(request.RoleIds.ToList(), cancellationToken);
            var adminRoleList = rolesIds.Select(id => new AdminRole
            {
                AdminId = admin.Id,
                RoleId = id
            }).ToList();
            await _context.AdminRoles.AddRangeAsync(adminRoleList, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _context.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            await _context.RollbackTransactionAsync(cancellationToken);
            return Result.Failure();
        }
    }

    // 过滤不允许的组别，避免越权
    private async Task<List<int>> CheckRolesAuth(List<int> roleIds, CancellationToken cancellationToken = default)
    {
        var childrenRoleIds = await _roleManager.GetChildrenRoleIds(_currentUserService.UserId, true, cancellationToken);
        roleIds = roleIds.Intersect(childrenRoleIds).ToList();
        if (roleIds.Count == 0)
        {
            throw new Exception("The parent group exceeds permission limit");
        }
        return roleIds;
    }
}
