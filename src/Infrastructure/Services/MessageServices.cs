using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Configurations;
using MailKit.Net.Smtp;
using MimeKit;

namespace CasseroleX.Infrastructure.Services;
public class MessageServices : IEmailSender, ISmsSender
{
    private readonly ISiteConfigurationService _siteConfigurationService;

    public MessageServices(ISiteConfigurationService siteConfigurationService)
    {
        _siteConfigurationService = siteConfigurationService;
    }

    public async Task SendEmailAsync(string email, string subject, string content)
    {
        // Plug in your email service here to send an email.
        var config = await _siteConfigurationService.GetConfigurationAsync<EmailConfigInfo>(); 
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(config.EmailDisplayname, config.EmailFrom));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = content
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        client.Connect(config.EmailHost, config.EmailPort, config.EmailSSL == 1);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        client.Authenticate(config.EmailUserName, config.EmailPassword);
        client.Send(message);
        client.Disconnect(true);
    }

    public Task SendSmsAsync(string number, string message)
    {
        // Plug in your SMS service here to send a text message.
        return Task.FromResult(0);
    }
}
