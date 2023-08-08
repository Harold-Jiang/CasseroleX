using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Users.Commands.UpdateUser;
public class UpdateUserCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string? Password { get; init; }

    public required string NickName { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string Mobile { get; set; }

    public int LoginFailure { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IEncryptionService _encryptionService;

    public UpdateUserCommandHandler(IApplicationDbContext context,
        IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(User), request.Id);

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
        entity.GroupId = request.GroupId;

        _context.Users.Update(entity);

        return (await _context.SaveChangesAsync(cancellationToken) > 0) ? Result.Success() : Result.Failure();

    }
}
