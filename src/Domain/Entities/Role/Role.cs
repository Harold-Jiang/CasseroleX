
namespace CasseroleX.Domain.Entities.Role;

/// <summary>
/// 角色表
/// </summary>
public partial class Role : BaseAuditableEntity
{
    /// <summary>
    /// 父角色
    /// </summary>
    public int Pid { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 规则ID
    /// </summary>
    public string Rules { get; set; } = null!;

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}
