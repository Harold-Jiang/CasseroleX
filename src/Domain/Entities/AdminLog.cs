using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasseroleX.Domain.Entities;

public class AdminLog : BaseAuditableEntity
{
    /// <summary>
    /// 管理员ID
    /// </summary> 
    public int AdminId { get; set; }
    /// <summary>
    /// 管理员用户名
    /// </summary>
    [StringLength(50)]
    public string UserName { get; set; } = null!;
    /// <summary>
    /// 操作的页面
    /// </summary>
    [StringLength(255)]
    public string? Url { get; set; }
    /// <summary>
    /// 日志标题
    /// </summary>
    [StringLength(100)]
    public string? Title { get; set; }
    /// <summary>
    /// 日志内容
    /// </summary>
    [Column(TypeName = "text")]
    public string? Content { get; set; }
    /// <summary>
    /// Ip地址
    /// </summary>
    [StringLength(50)]
    public string? Ip { get; set; }
    /// <summary>
    /// User-Agent
    /// </summary>
    [StringLength(255)]
    public string? UserAgent { get; set; }
     
}