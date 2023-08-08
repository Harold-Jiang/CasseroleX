using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Users.Commands.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("User name cannot be empty")
            .Matches(@"\w{3,30}").WithMessage("Incorrect user name format")
            .MustAsync(UserNameExists)
            .WithMessage("User name already exists");

        RuleFor(v => v.Password)
            .Matches(@"\S{32}").WithMessage("Incorrect password format");

        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailExists)
            .WithMessage("Email already exists"); ;

        RuleFor(v => v.NickName).NotEmpty();

        RuleFor(v => v.Mobile)
            .NotEmpty().WithMessage("Mobile cannot be empty")
            .Matches(@"^1[3-9]\d{9}$").WithMessage("Incorrect mobile format")
            .MustAsync(MobileExists)
            .WithMessage("Mobile already exists");
    }


    private async Task<bool> UserNameExists(UpdateUserCommand command, string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Users.CountAsync(a => a.UserName == userName && a.Id != command.Id, cancellationToken) == 0;
    }

    private async Task<bool> EmailExists(UpdateUserCommand command, string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.CountAsync(a => a.Email == email && a.Id != command.Id, cancellationToken) == 0;
    }

    private async Task<bool> MobileExists(UpdateUserCommand command, string mobile, CancellationToken cancellationToken = default)
    {
        return await _context.Users.CountAsync(a => a.Mobile == mobile && a.Id != command.Id, cancellationToken) == 0;
    }
}