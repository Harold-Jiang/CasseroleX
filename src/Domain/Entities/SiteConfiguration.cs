namespace CasseroleX.Domain.Entities;

/// <summary>
/// 系统配置
/// </summary>
public class SiteConfiguration : BaseAuditableEntity
{ 
    /// <summary>
    /// 变量名
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 分组
    /// </summary>
    public string Group { get; set; } = null!;
    /// <summary>
    /// 变量标题
    /// </summary>
    public string? Title { get; set; }
    /// <summary>
    /// 变量描述
    /// </summary>
    public string? Tip { get; set; }
    /// <summary>
    /// 变量类型:string,text,int,bool,array,datetime,date,file
    /// </summary> 
    public string? Type { get; set; }
    /// <summary>
    /// 可见条件
    /// </summary> 
    public string? Visible { get; set; }
    /// <summary>
    /// 变量值
    /// </summary>
    public string? Value { get; set; }
    /// <summary>
    /// 变量字典数据
    /// </summary>
    public string? Content { get; set; }
    /// <summary>
    /// 验证规则
    /// </summary> 
    public string? Rule { get; set; }
    /// <summary>
    /// 扩展属性
    /// </summary> 
    public string? Extend { get; set; }
}
