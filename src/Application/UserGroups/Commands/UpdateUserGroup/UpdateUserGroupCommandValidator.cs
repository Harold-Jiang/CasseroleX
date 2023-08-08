using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserGroups.Commands.UpdateUserGroup;
public class UpdateUserGroupCommandValidator : AbstractValidator<UpdateUserGroupCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateUserGroupCommandValidator(IApplicationDbContext context)
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

    private async Task<bool> NameExists(UpdateUserGroupCommand command,string name, CancellationToken cancellationToken = default)
    {
        return await _context.UserGroups.CountAsync(a => a.Name == name && command.Id  != a.Id, cancellationToken) == 0;
    }
}