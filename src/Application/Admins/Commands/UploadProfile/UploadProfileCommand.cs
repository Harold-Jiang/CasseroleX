using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Admins.Commands.UploadProfile;
public class UploadProfileCommand : IRequest<Result>
{
    public int Id { get; set; }
    
    public string? Password { get; init; }
     
    public required string NickName { get; set; }

    public required string Email { get; set; } 
     
}
public class UploadProfileCommandHandler : IRequestHandler<UploadProfileCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly IRoleManager _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public UploadProfileCommandHandler(IApplicationDbContext context,
        IEncryptionService encryptionService,
        ICurrentUserService currentUserService,
        IRoleManager roleManager)
    {
        _context = context;
        _encryptionService = encryptionService;
        _currentUserService = currentUserService;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(UploadProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Admins
            .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(Admin), request.Id);

        //edit password
        if (request.Password.IsNotNullOrEmpty())
        {
            var salt = _encryptionService.CreateSaltKey(6);
            entity.PasswordHash = _encryptionService.CreatePasswordHash(request.Password, salt);
        }

        entity.NickName = request.NickName;
        entity.Email = request.Email;

        _context.Admins.Update(entity);

        return (await _context.SaveChangesAsync(cancellationToken) > 0) ? Result.Success() : Result.Failure();

    } 
}