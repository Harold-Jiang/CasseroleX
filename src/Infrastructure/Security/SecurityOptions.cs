namespace CasseroleX.Infrastructure.Security;

public class SecurityOptions
{
    /// <summary>
    /// Auth过期时间
    /// </summary>
    public int CookieAuthExpires { get; set; }

    /// <summary>
    /// Cookie prefix
    /// </summary>
    public string? CookiePrefix { get; set; }

    /// <summary>
    /// Cookie claim issuer 
    /// </summary>
    public string? CookieClaimsIssuer { get; set; }


    /// <summary>
    /// 获取或设置“Cookie SecurePolicy Always”的值
    /// </summary>
    public bool CookieSecurePolicyAlways { get; set; }


}
