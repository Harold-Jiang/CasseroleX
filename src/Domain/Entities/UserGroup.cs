namespace CasseroleX.Domain.Entities;
public class UserGroup :BaseAuditableEntity
{
    /// <summary>
    /// 组名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 权限节点
    /// </summary>
    public string? Rules { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}
