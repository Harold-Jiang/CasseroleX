using System.ComponentModel.DataAnnotations;

namespace CasseroleX.Domain.Enums;

/// <summary>
/// 操作类型
/// </summary>
public enum ActionType
{
    #region 权限操作
    /// <summary>
    /// 添加
    /// </summary>
    [Display(Name = "添加")]
    Add = 99,
    /// <summary>
    /// 删除
    /// </summary>
    [Display(Name = "删除")]
    Delete = 98,
    /// <summary>
    /// 修改
    /// </summary>
    [Display(Name = "修改")]
    Edit = 97,
    /// <summary>
    /// 查看
    /// </summary>
    [Display(Name = "查看")]
    View = 96,
    /// <summary>
    /// 显示
    /// </summary>
    [Display(Name = "详情")]
    Detail = 95,
    /// <summary>
    /// 审核
    /// </summary>
    [Display(Name = "审核")]
    Audit = 94,
    /// <summary>
    /// 刷新
    /// </summary>
    [Display(Name = "刷新")]
    Refresh = 93,
    /// <summary>
    /// 导入
    /// </summary>
    [Display(Name = "导入")]
    Import = 92,
    /// <summary>
    /// 批量
    /// </summary>
    [Display(Name = "批量操作")]
    Multi = 91,
    /// <summary>
    /// 还原
    /// </summary>
    [Display(Name = "还原")]
    Restore = 90,
    /// <summary>
    /// 销毁
    /// </summary>
    [Display(Name = "销毁")]
    Destroy = 89,
    /// <summary>
    /// 回收
    /// </summary>
    [Display(Name = "回收")]
    Recycle = 88,
    #endregion

}
