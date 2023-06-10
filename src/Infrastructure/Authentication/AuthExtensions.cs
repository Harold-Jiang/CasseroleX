using System.Security.Claims;
using System.Security.Principal;
using CasseroleX.Application.Utils;

namespace CasseroleX.Infrastructure.Authentication;
public static class AuthExtensions
{
    public const string Avatar = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/avatar";
    public const string LoginTime = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/logintime";
    public const string RolePermissonIds = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rolepermissonids";
    

    public static bool IsSuperAdmin(this IIdentity? identity)
    {
        if (identity == null) return false;
        var claim = ((ClaimsIdentity)identity).Claims.Where(t => t.Type == RolePermissonIds && t.Value == "*").FirstOrDefault();
        return claim != null;
    }

    public static List<int> GetRoleIds(this IIdentity? identity)
    {
        if (identity == null) return new();

        var claim = ((ClaimsIdentity)identity).Claims.Where(t => t.Type == ClaimTypes.Role).FirstOrDefault();
        return claim != null ? claim.Value.ToIList<int>() : new();
    }
    
    public static List<string> GetPermissionIds(this IIdentity? identity)
    {
        if (identity == null) return new();
        var claim = ((ClaimsIdentity)identity).Claims.Where(t => t.Type == RolePermissonIds ).FirstOrDefault();
        return claim != null ? claim.Value.ToIList<string>() : new();
    }

    public static int ID(this IIdentity? identity)
    {
        if (identity == null) return 0;
        var claim = ((ClaimsIdentity)identity).Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
        return claim != null ? claim.Value.ToInt() : 0;
    }

    public static string GetAvatar(this IIdentity? identity)
    {
        if (identity == null) return string.Empty;
        var claim = ((ClaimsIdentity)identity).FindFirst(Avatar);
        return claim != null ? claim.Value : string.Empty;
    }

    public static string GetLoginTime(this IIdentity? identity)
    {
        if (identity == null) return string.Empty;
        var claim = ((ClaimsIdentity)identity).FindFirst(LoginTime);
        return claim != null ? claim.Value : string.Empty;
    }
}
