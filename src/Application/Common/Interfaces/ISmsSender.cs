namespace CasseroleX.Application.Common.Interfaces;
public interface ISmsSender
{
    Task SendSmsAsync(string number, string message);
}
