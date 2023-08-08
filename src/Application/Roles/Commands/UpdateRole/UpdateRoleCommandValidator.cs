using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Commands.UpdateRole;
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Rules)
            .NotEmpty()
            .MustAsync(NameExists).WithMessage("Name already exists");
    }

    private async Task<bool> NameExists(UpdateRoleCommand command,string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles.CountAsync(a => a.Name == name && command.Id != a.Id, cancellationToken) == 0;
    }

}
