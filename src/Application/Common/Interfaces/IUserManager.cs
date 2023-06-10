using CasseroleX.Domain.Common;

namespace CasseroleX.Application.Common.Interfaces;
public interface IUserManager  
{
    Task<T?> GetUserByEmailAsync<T>(string? email, CancellationToken cancellationToken = default) where T : BaseUser;
    Task<T?> GetUserByIdAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseUser;
    Task<T?> GetUserByMobileAsync<T>(string? mobile, CancellationToken cancellationToken = default) where T : BaseUser;
    Task<T?> GetUserByUserNameAsync<T>(string? userName, CancellationToken cancellationToken = default) where T : BaseUser;
    Task<bool> UpdateAsync<T>(T entity, string[]? propertyNames = null, CancellationToken cancellationToken = default) where T : BaseUser;
}