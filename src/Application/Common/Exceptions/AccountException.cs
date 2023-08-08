namespace CasseroleX.Application.Common.Exceptions;

// 定义验证码异常基类
public class AccountException : Exception
{
    public AccountException(string message) : base(message) { }
}
 