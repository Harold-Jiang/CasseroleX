using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserRules.Commands.CreateUserRule;
public class CreateUserRuleCommandValidator : AbstractValidator<CreateUserRuleCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateUserRuleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(menu => menu.Name)
          .NotEmpty().WithMessage("Name is required")
          .Matches(@"^[a-z0-9_\/]+$").WithMessage("URL rule can only contain lowercase letters, numbers, underscore, and slash")
          .MustAsync(NameExists).WithMessage("Name already exists");

        RuleFor(rule => rule.Title)
            .NotEmpty().WithMessage("Title is required");
    }

    private async Task<bool> NameExists(string name, CancellationToken cancellationToken = default)
    {
        return await _context.UserRules.CountAsync(a => a.Name == name, cancellationToken) == 0;
    }

} 
