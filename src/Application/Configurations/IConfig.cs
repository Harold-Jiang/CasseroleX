namespace CasseroleX.Application.Configurations;

/// <summary>
/// 应用程序设置中的配置接口
/// </summary>
public partial interface IConfig
{
    /// <summary>
    /// 配置名称
    /// </summary>
    string ConfigTypeName => GetType().Name;

    /// <summary>
    /// 排序
    /// </summary>
    /// <returns>Order</returns>
    public int GetOrder() => 1;
}