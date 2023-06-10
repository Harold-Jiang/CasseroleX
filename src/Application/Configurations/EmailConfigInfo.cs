using System.ComponentModel.DataAnnotations;

namespace CasseroleX.Application.Configurations;

/// <summary>
/// 邮件配置信息类
/// </summary> 
public partial class EmailConfigInfo : IConfig
{
    public const string Postion = "email";

    /// <summary>
    /// STMP服务器
    /// </summary>
    public string EmailHost { get; set; } = null!;

    /// <summary>
    /// SMTP端口
    /// </summary>
    public int EmailPort { get; set; }
    /// <summary>
    /// 是否启用SSL加密连接 [\"无\",\"SSL\"]0 1 
    /// </summary>
    public int EmailSSL { get; set; }

    /// <summary>
    /// 邮箱账号
    /// </summary>
    public string EmailUserName { get; set; } = null!;

    /// <summary>
    /// 邮箱密码
    /// </summary>
    public string EmailPassword { get; set; } = null!;

    /// <summary>
    /// 发送邮箱
    /// </summary>
    public string EmailFrom { get; set; } = null!;

    /// <summary>
    /// 发送邮箱的显示名
    /// </summary>
    public string EmailDisplayname { get; set; } = null!;
    /// <summary>
    /// 邮件验证码有效期(分钟)须大于0
    /// </summary>
    [Display(Name = "验证码有效期")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int RegEmailExpired { get; set; } = 1;

    /// <summary>
    /// 邮件验证码生成位数(位)
    /// </summary>
    [Display(Name = "验证码位数")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int RegEmailCodeLength { get; set; } = 4;


}
