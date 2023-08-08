namespace CasseroleX.Application.Common.Exceptions;

// 定义验证码异常基类
public abstract class VerificationCodeException : Exception
{
    public VerificationCodeException(string message) : base(message) { }
}

// 图形验证码异常
public class ImageVerificationCodeException : VerificationCodeException
{
    public ImageVerificationCodeException(string message) : base(message) { }
}

// 短信验证码异常
public class SmsVerificationCodeException : VerificationCodeException
{
    public SmsVerificationCodeException(string message) : base(message) { }
}
