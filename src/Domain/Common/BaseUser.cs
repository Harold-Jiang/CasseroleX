namespace CasseroleX.Domain.Common;
public class BaseUser : BaseAuditableEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// 密码盐
    /// </summary>
    public string Salt { get; set; } = null!;

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? Mobile { get; set; }


    /// <summary>
    /// 失败次数
    /// </summary>
    public byte LoginFailure { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime? LoginTime { get; set; }

    /// <summary>
    /// 登录IP
    /// </summary>
    public string? LoginIp { get; set; }

    /// <summary>
    /// Session标识
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// 状态 normal正常 hidden隐藏
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool LockoutEnabled { get; set; }

    /// <summary>
    /// 锁定时间
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set;}

    /// <summary>
    /// 是否开启2步登录
    /// </summary>
    public bool TwoFactorEnabled { get; set; }
}
