using CasseroleX.Domain.Common;

namespace CasseroleX.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    string UserName { get; }
    bool IsSuperAdmin { get; }
    List<int> RoleIds { get; }
    List<string> PermissionIds { get; }
    Task<int> CheckPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
    Task<T?> GetUserAsync<T>() where T : BaseUser;
}
