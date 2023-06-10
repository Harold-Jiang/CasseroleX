namespace CasseroleX.Application.Common.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task<T?> GetAsync<T>(string key, Func<Task<T>> dataRetriever, int? cacheTime = null, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
    Task SetAsync(string key, object? data, int? cacheTime = null, CancellationToken cancellationToken = default);
}
