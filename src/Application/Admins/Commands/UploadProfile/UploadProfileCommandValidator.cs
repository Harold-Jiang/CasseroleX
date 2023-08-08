using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Commands.UploadProfile;
public class UploadProfileCommandValidator : AbstractValidator<UploadProfileCommand>
{
    private readonly IApplicationDbContext _context;

    public UploadProfileCommandValidator(IApplicationDbContext context)
    {
        _context = context;
         
        RuleFor(v => v.Password)
            .Matches(@"\S{32}").WithMessage("Incorrect password format");

        RuleFor(v => v.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailExists)
            .WithMessage("Email already exists");

        RuleFor(v => v.NickName)
           .NotEmpty() 
           .MustAsync(NickNameExists)
           .WithMessage("NickName already exists");



    }

    private async Task<bool> NickNameExists(UploadProfileCommand command, string nickName, CancellationToken cancellationToken = default)
    {
        return await _context.Admins.CountAsync(a => a.NickName == nickName && a.Id != command.Id, cancellationToken) == 0;
    }

    private async Task<bool> EmailExists(UploadProfileCommand command, string email, CancellationToken cancellationToken = default)
    {
        return await _context.Admins.CountAsync(a => a.Email == email && a.Id != command.Id, cancellationToken) == 0;
    }

}
