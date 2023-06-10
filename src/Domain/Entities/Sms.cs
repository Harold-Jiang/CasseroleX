namespace CasseroleX.Domain.Entities;

/// <summary>
/// 短信验证码表
/// </summary> 
public partial class Sms 
{
    public int Id { get; set; }
    /// <summary>
    /// 事件
    /// </summary>
    public string? Event { get; set; }
    /// <summary>
    /// 手机号
    /// </summary>
    public string? Mobile { get; set; }
    /// <summary>
    /// 验证码
    /// </summary>
    public string? Code { get; set; }
    /// <summary>
    /// 验证次数
    /// </summary>
    public int Times { get; set; }
    /// <summary>
    /// IP
    /// </summary>
    public string? Ip { get; set; }

    public DateTime CreateTime { get; set; }

}
