namespace CasseroleX.Domain.Exceptions;

// 定义验证码异常基类
public class AccountException : JsonResultException
{
    public AccountException(string message) : base(message) { }
}
 