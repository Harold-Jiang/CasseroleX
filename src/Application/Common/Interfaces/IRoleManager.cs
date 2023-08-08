using CasseroleX.Application.Roles.Queries;

namespace CasseroleX.Application.Common.Interfaces;
public interface IRoleManager
{
    Task<HashSet<string>> GetPermissionsAsync(int adminId, CancellationToken cancellationToken = default);
    Task<bool> CheckPermissionAsync(int adminId, string permissionName, CancellationToken cancellationToken = default);
    Task<List<int>> GetChildrenRoleIds(int adminId, bool withself = false, CancellationToken cancellationToken = default);
    Task<(List<string>, List<int>)> GetRolePermissionIdsAsync(int adminId, CancellationToken cancellationToken = default);
    Task<List<int>> GetChildrenAdminIds(bool isSuperAdmin, int adminId, bool withself = false, CancellationToken cancellationToken = default);
    Task<List<RoleDto>> GetRolesAsync(int adminId, CancellationToken cancellationToken = default);
}
