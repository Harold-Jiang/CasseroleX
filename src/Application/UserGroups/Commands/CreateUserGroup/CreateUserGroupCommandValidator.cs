using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserGroups.Commands.CreateUserGroup;
public class CreateUserGroupCommandValidator : AbstractValidator<CreateUserGroupCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateUserGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Rules)
            .NotEmpty();

        RuleFor(v => v.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .MustAsync(NameExists).WithMessage("Name already exists");
    }

    private async Task<bool> NameExists(string name, CancellationToken cancellationToken = default)
    {
        return await _context.UserGroups.CountAsync(a => a.Name == name, cancellationToken) == 0;
    }
}