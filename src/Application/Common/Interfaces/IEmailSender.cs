namespace CasseroleX.Application.Common.Interfaces;
public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}