namespace CasseroleX.Application.Common.Interfaces;
public interface IRoleManager
{
    Task<HashSet<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> CheckPermissionAsync(int userId, string permissionName, CancellationToken cancellationToken = default);
    Task<List<int>> GetChildrenRoleIds(int userId, bool withself = false, CancellationToken cancellationToken = default);
    Task<(List<string>, List<int>)> GetRolePermissionIdsAsync(int userId, CancellationToken cancellationToken = default);
}
