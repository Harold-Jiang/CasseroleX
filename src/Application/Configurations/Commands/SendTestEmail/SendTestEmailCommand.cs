using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;

namespace CasseroleX.Application.Configurations.Commands.SendTestEmail;
public record SendTestEmailCommand(string ReceiverMail) :IRequest<Result>;


public class SendTestEmailCommandHandler : IRequestHandler<SendTestEmailCommand, Result>
{
    private readonly IEmailSender _sender;

    public SendTestEmailCommandHandler(IEmailSender sender)
    {
        _sender = sender;
    }

    public async Task<Result> Handle(SendTestEmailCommand request, CancellationToken cancellationToken)
    {
        if (!request.ReceiverMail.IsNotNullOrEmpty())
            throw new ArgumentNullException(nameof(request.ReceiverMail));

        try
        {
            await _sender.SendEmailAsync(request.ReceiverMail, "Email Test!", "This is a test email!");
            return Result.Success();
        }
        catch { }
        return Result.Failure();
       
    }
}
