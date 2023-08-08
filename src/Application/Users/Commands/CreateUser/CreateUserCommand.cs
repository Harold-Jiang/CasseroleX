using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using MediatR;

namespace CasseroleX.Application.Users.Commands.CreateUser;
public class CreateUserCommand : IRequest<Result>
{
    public required int[] RoleIds { get; init; }

    public required string Password { get; init; }

    public required string NickName { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Mobile { get; set; }

    public required Status Status { get; set; }

}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly IRoleManager _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public CreateUserCommandHandler(IApplicationDbContext context,
        IEncryptionService encryptionService,
        IRoleManager roleManager,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _encryptionService = encryptionService;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var salt = _encryptionService.CreateSaltKey(6);
        var user = new User
        {
            Salt = salt,
            PasswordHash = _encryptionService.CreatePasswordHash(request.Password, salt),
            Avatar = "",
            UserName = request.UserName,
            NickName = request.NickName,
            Email = request.Email,
            Mobile = request.Mobile,
            Status = request.Status
        };
        using var transaction = _context.BeginTransactionAsync(cancellationToken);
        try
        {
            // save user 
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

         

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
 
}
