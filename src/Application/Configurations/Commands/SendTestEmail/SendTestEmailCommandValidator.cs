using FluentValidation;

namespace CasseroleX.Application.Configurations.Commands.SendTestEmail;
public class SendTestEmailCommandValidator : AbstractValidator<SendTestEmailCommand>
{
    public SendTestEmailCommandValidator()
    {
        RuleFor(v => v.ReceiverMail)
         .NotEmpty()
         .EmailAddress()
         .WithMessage("Incorrect email format"); 
    }
}
