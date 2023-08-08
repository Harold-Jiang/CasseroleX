using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Commands.UpdateMenuCommand;
public class UpdateMenuCommandValidator : AbstractValidator<UpdateMenuCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateMenuCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(menu => menu.Name)
          .NotEmpty().WithMessage("Name is required")
          .Matches(@"^[a-z0-9_\/]+$").WithMessage("URL rule can only contain lowercase letters, numbers, underscore, and slash")
          .MustAsync(NameExists).WithMessage("Name already exists");

        RuleFor(rule => rule.Title)
            .NotEmpty().WithMessage("Title is required");
    }

    private async Task<bool> NameExists(UpdateMenuCommand command,string name, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions.CountAsync(a => a.Name == name && a.Id != command.Id, cancellationToken) == 0;
    }
}
