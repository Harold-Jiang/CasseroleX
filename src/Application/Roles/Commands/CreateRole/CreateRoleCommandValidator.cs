using FluentValidation;

namespace CasseroleX.Application.Roles.Commands.CreateRole;
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Rules)
            .MaximumLength(200)
            .NotEmpty();
        RuleFor(v => v.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}
