using FluentValidation;

namespace CasseroleX.Application.Roles.Commands.UpdateRole;
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Rules)
            .MaximumLength(200)
            .NotEmpty();
    }
}
