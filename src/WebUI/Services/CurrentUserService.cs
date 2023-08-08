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

    public List<string> PermissionIds => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(AuthExtensions.RolePermissonIds)).ToIList<string>();

    public List<int> RoleIds => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)).ToIList<int>();

    public bool IsSuperAdmin => this.PermissionIds.Contains("*");

    public async Task<T?> GetUserAsync<T>() where T : BaseUser
    {
        var user = await _authenticationService.GetAuthenticatedUser<T>();
        return user as T;
    }

    public async Task<int> CheckPermissionAsync(string permissionName, CancellationToken cancellationToken = default)
    {
        return await _roleManager.CheckPermissionAsync(this.UserId, permissionName, cancellationToken) ? 1 : 0;
    }
}
