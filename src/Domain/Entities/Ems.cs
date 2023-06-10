namespace  CasseroleX.Domain.Entities;

/// <summary>
/// 邮箱验证码表
/// </summary>
public partial class Ems
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 事件
    /// </summary>
    public string? Event { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

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

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

}
