using Microsoft.AspNetCore.Http;

namespace CasseroleX.Infrastructure.Authentication;
public class CookieAuthenticationDefaults
{
    /// <summary>
    /// The default value used for authentication scheme
    /// </summary>
    public const string AuthenticationScheme = "CasseroleXAuthentication";

    /// <summary>
    /// The default value used for external authentication scheme
    /// </summary>
    public const string ExternalAuthenticationScheme = "ExternalAuthentication";

    /// <summary>
    /// The default value for the login path
    /// </summary>
    public static readonly PathString LoginPath = new("/Account/Login");

    /// <summary>
    /// The default value for the access denied path
    /// </summary>
    public static readonly PathString AccessDeniedPath = new("/Error/NoAuthorize");
}
