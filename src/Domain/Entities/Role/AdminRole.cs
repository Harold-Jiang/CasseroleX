namespace CasseroleX.Domain.Entities.Role;

/// <summary>
/// 用户角色表
/// </summary>
public partial class AdminRole
{
    /// <summary>
    /// 会员ID
    /// </summary>
    public int AdminId { get; set; }

    /// <summary>
    /// 角色ID
    /// </summary>
    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
    public virtual Admin Admin { get; set; } = null!;
}
