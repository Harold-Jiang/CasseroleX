using System.Security.Claims;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Common;
using CasseroleX.Domain.Enums;
using CasseroleX.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.Authentication;

/// <summary>
/// Represents service using cookie middleware for the authentication
/// </summary>
public class CookieAuthenticationService : ICustomAuthenticationService
{ 
    private readonly SecurityOptions _securityConfig;
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;

    private string CookieUserName => $"{_securityConfig.CookiePrefix}CookieName";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private BaseUser? _cachedUser;


    public CookieAuthenticationService(
        IHttpContextAccessor httpContextAccessor,
        IOptions<SecurityOptions> securityConfig,
        IUserManager userManager,
        IRoleManager roleManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _securityConfig = securityConfig.Value;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    #region Methods

    /// <summary>
    /// Is Signed In
    /// </summary>
    /// <returns>bool</returns>
    public virtual bool IsSignedIn()
    {
        ArgumentNullException.ThrowIfNull(_httpContextAccessor.HttpContext);
        var principal = _httpContextAccessor.HttpContext.User;
        return principal.Identities != null &&
            principal.Identities.Any(i => i.AuthenticationType == CookieAuthenticationDefaults.AuthenticationScheme);
    }


    /// <summary>
    /// Sign in
    /// </summary> 
    public virtual async Task SignIn<T>(T user, bool isPersistent) where T : BaseUser
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        //add userId
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer, _securityConfig.CookieClaimsIssuer)
        };
        //add userName
        if (!string.IsNullOrEmpty(user.UserName))
            claims.Add(new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));

        //add token   
        if (string.IsNullOrEmpty(user.Token))
        {
            user.Token = Guid.NewGuid().ToString("N").ToLower();
            await _userManager.UpdateAsync(user, new string[] { "Token" });
            claims.Add(new Claim(ClaimTypes.Hash, user.Token, ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));
        }
        else
            claims.Add(new Claim(ClaimTypes.Hash, user.Token, ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));

        //add permissionIds 
        var (permissions,roleIds)= await _roleManager.GetRolePermissionIdsAsync(user.Id);
        claims.Add(new Claim(AuthExtensions.RolePermissonIds,string.Join(",", permissions), ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));

        //add roleIds
        claims.Add(new Claim(ClaimTypes.Role, string.Join(",", roleIds), ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));


        //add Avatar
        claims.Add(new Claim(AuthExtensions.Avatar,user.Avatar??"", ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));

        //add Logintime
        claims.Add(new Claim(AuthExtensions.LoginTime, user.LoginTime.ToDateTimeString(true), ClaimValueTypes.String, _securityConfig.CookieClaimsIssuer));


        //create principal for the present scheme of authentication
        var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var userPrincipal = new ClaimsPrincipal(userIdentity);

        //set value that indicates whether the session is persisted and the time at which the authentication was issued
        var authenticationProperties = new AuthenticationProperties
        {
            IsPersistent = isPersistent,
            IssuedUtc = DateTime.UtcNow,
            ExpiresUtc = DateTime.UtcNow.AddHours(_securityConfig.CookieAuthExpires)
        };

        //sign in user
        if (_httpContextAccessor.HttpContext != null)
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

        //cache authenticated user
        _cachedUser = user;
    }

    /// <summary>
    /// Sign out user
    /// </summary>
    public virtual async Task SignOut()
    {
        //Firstly reset cached user
        _cachedUser = null;

        //and then sign out user from the present scheme of authentication
        if (_httpContextAccessor.HttpContext != null)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults
                .AuthenticationScheme);
            //sign out also from other schema
            //await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults
            // .ExternalAuthenticationScheme);
        }
    } 

    /// <summary>
    /// Get an authenticated user
    /// </summary>
    /// <returns>User</returns>
    public virtual async Task<T?> GetAuthenticatedUser<T>() where T : BaseUser
    {
        //check if there is a cached user
        if (_cachedUser != null)
            return _cachedUser as T;

        //get the authenticated user identity
        if (_httpContextAccessor.HttpContext == null) return _cachedUser as T;
        var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
            return null;

        BaseUser? user = null;

        var userIdClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier
            && claim.Issuer.Equals(_securityConfig.CookieClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
        if (userIdClaim != null)
            user = await _userManager.GetUserByIdAsync<T>(userIdClaim.Value.ToInt());

        if (user != null)
        {
            var tokenClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Hash
                && claim.Issuer.Equals(_securityConfig.CookieClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (tokenClaim == null || tokenClaim.Value != user.Token)
            {
                user = null;
            }
        }
        //Check if the found user is available
        if (user is not { Status: Status.normal } || user.LockoutEnabled)
            return null;

        //Cache the authenticated user
        _cachedUser = user;

        return _cachedUser as T;
    }

    /// <summary>
    /// Get userId by cookie
    /// </summary>
    /// <returns>String value of cookie</returns>
    public virtual Task<string> GetUserByCookies()
    {
        return _httpContextAccessor.HttpContext?.Request == null ? Task.FromResult("") : Task.FromResult(_httpContextAccessor.HttpContext.Request.Cookies[CookieUserName] ?? "");
    }

    /// <summary>
    /// Set userId to cookie
    /// </summary>
    public virtual Task SetCookieUser(int userId)
    {
        if (_httpContextAccessor.HttpContext?.Response == null)
            return Task.CompletedTask;

        //Delete existing cookie value
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieUserName);

        //Get the date date of current cookie expiration
        var cookieExpiresDate = DateTime.UtcNow.AddHours(_securityConfig.CookieAuthExpires);

        //If provided guid is empty (only remove cookies)
        if (userId == 0)
            return Task.CompletedTask;

        //set new cookie value
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = cookieExpiresDate
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieUserName, userId.ToString(), options);

        return Task.CompletedTask;
    }
    #endregion
}
