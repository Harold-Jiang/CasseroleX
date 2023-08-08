using CasseroleX.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Configurations.Commands.CreateConfigCommand;
public class CreateConfigCommandValidator : AbstractValidator<CreateConfigCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateConfigCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
           .NotEmpty().WithMessage("Name cannot be empty")
           .MustAsync(NameExists)
           .WithMessage("Name already exists");
    }
    private async Task<bool>  NameExists(string name, CancellationToken cancellationToken = default)
    {
        return await _context.SiteConfigurations.CountAsync(a => a.Name.ToLower() == name.ToLower(), cancellationToken) == 0;
    }

}
