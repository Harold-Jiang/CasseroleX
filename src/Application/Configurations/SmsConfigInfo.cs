using System.ComponentModel.DataAnnotations;

namespace CasseroleX.Application.Configurations;

/// <summary>
/// 短信平台设置
/// </summary>
public partial class SmsConfigInfo : IConfig
{

    #region 短信平台设置==================================
    /// <summary>
    /// 短信API地址
    /// </summary>
    public string SmsApiUrl { get; set; } = "";

    /// <summary>
    /// 短信平台登录AppId
    /// </summary
    [Display(Name = "短信平台AppId")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string SmsSdkAppId { get; set; } = "";


    /// <summary>
    /// 短信云平台SecretId(腾讯云)
    /// </summary>
    [Display(Name = "腾讯云SecretId")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string SmsSecretId { get; set; } = "";

    /// <summary>
    /// 短信平台SecretKey(腾讯云)
    /// </summary>
    [Display(Name = "腾讯云SecretKey")]
    [Required(ErrorMessage = "{0}不可为空")]
    public string SmsSecretKey { get; set; } = "";


    /// <summary>
    /// 手机验证码间隔限制(分钟)须大于0
    /// </summary>
    [Display(Name = "验证码间隔限制")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int RegSmsCodeCtrl { get; set; } = 2;

    /// <summary>
    /// 手机验证码有效期(分钟)须大于0
    /// </summary>
    [Display(Name = "验证码有效期")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int RegSmsExpired { get; set; } = 10;

    /// <summary>
    /// 手机验证码生成位数(位)
    /// </summary>
    [Display(Name = "手机验证码位数")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int RegSmsCodeLength { get; set; } = 4;


    /// <summary>
    /// 最大允许检测的次数
    /// </summary>
    [Display(Name = "最大允许检测的次数")]
    [Required(ErrorMessage = "{0}不可为空")]
    public int MaxCheckNums { get; set; } = 10;


    public string SignName { get; set; } = "车销量";
    #endregion
}
