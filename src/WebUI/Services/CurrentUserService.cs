using System.Security.Claims;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Common;
using CasseroleX.Infrastructure.Authentication;

namespace WebUI.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly ICustomAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoleManager _roleManager;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor,
        ICustomAuthenticationService authenticationService,
        IRoleManager roleManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _authenticationService = authenticationService;
        _roleManager = roleManager;
    }

    public int UserId => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)).ToInt();
    public string UserName => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name))?.ToString() ?? "";
    public int PermissionIds => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(AuthExtensions.RolePermissonIds)).ToInt();

    //public string Roles => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role))?.ToString()??"";

   
    public async Task<T?> GetUserAsync<T>() where T : BaseUser
    {
        var user = await _authenticationService.GetAuthenticatedUser<T>();
        return user as T;
    }

    public async Task<bool> CheckPermissionAsync(string permissionName,CancellationToken cancellationToken =default)
    {
        return await _roleManager.CheckPermissionAsync(this.UserId, permissionName,cancellationToken);
    }
}
