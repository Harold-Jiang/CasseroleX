using CasseroleX.Domain.Common;

namespace CasseroleX.Application.Common.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    string UserName { get; }
    int PermissionIds { get; }
    Task<bool> CheckPermissionAsync(string permissionName, CancellationToken cancellationToken = default);
    Task<T?> GetUserAsync<T>() where T : BaseUser;
}
