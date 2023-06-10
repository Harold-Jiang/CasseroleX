using CasseroleX.Application.Common.Interfaces;

namespace CasseroleX.Infrastructure.Services;
public class MessageServices : IEmailSender, ISmsSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        // Plug in your email service here to send an email.
        return Task.FromResult(0);
    }

    public Task SendSmsAsync(string number, string message)
    {
        // Plug in your SMS service here to send a text message.
        return Task.FromResult(0);
    }
}
