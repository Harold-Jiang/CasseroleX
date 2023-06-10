namespace CasseroleX.Domain.Entities.Role;

/// <summary>
/// 角色权限表
/// </summary>
public partial class RolePermissions : BaseAuditableEntity
{

    /// <summary>
    /// menu为菜单,file为权限节点
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// 父ID
    /// </summary>
    public int Pid { get; set; }

    /// <summary>
    /// 规则名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 规则名称
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 规则URL
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 条件
    /// </summary>
    public string? Condition { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 是否为菜单
    /// </summary>
    public byte IsMenu { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    public string? MenuType { get; set; }

    /// <summary>
    /// 扩展属性
    /// </summary>
    public string? Extend { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    public string? Py { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    public string? PinYin { get; set; }

    /// <summary>
    /// 权重
    /// </summary>
    public int Weigh { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public Status Status { get; set; }
}
