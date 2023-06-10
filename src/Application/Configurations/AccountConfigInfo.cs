namespace CasseroleX.Application.Configurations;

public class AccountConfigInfo : IConfig
{
    /// <summary>
    /// 新用户注册设置 
    /// </summary> 
    public bool RegStatus { get; set; } = true;

    /// <summary>
    /// 新用户注册审核  
    /// </summary> 
    public bool RegVerify { get; set; } = false;

    /// <summary>
    /// 用户名保留关健字
    /// </summary> 
    public string RegKeywords { get; set; } = "admin,administrator,test";

    /// <summary>
    /// IP注册间隔限制0不限制(小时)
    /// </summary> 
    public int RegCtrl { get; set; } = 0;

    /// <summary>
    /// 登录验证码
    /// </summary>
    public bool LoginCaptcha { get; set; } = false;
    /// <summary>
    /// 登录失败超过10次则1天后重试
    /// </summary>
    public bool LoginFailureRetry { get; set; }
    /// <summary>
    /// 是否开启IP变动检测
    /// </summary>
    public bool LoginIpCheck { get; set; } = false;

    /// <summary>
    /// 是否开启两步授权
    /// </summary>
    public bool TwoFactorAuthenticationEnabled { get; set; } = false;

}
