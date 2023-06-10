using System.Collections.Concurrent;
using System.Text.Json;
using CasseroleX.Application.Common.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.Caching;

public class CacheService : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    private readonly IDistributedCache _distributedCache;
    private readonly RedisOptions _redisOptions;

    public CacheService(IDistributedCache distributedCache,
        IOptions<RedisOptions> redisOptions)
    {
        _distributedCache = distributedCache;
        _redisOptions = redisOptions.Value;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) 
    {
        string? cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cacheValue is null)
            return default;

        //deserialize item
        var item = JsonSerializer.Deserialize<T>(cacheValue);
        if (item == null)
            return default;

        return item;
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> dataRetriever, int? cacheTime = null, CancellationToken cancellationToken = default) 
    {
        var item = await GetAsync<T?>(key,cancellationToken);
        if (item is not null)
            return item;

        //or create it using passed function
        var result = await dataRetriever();

        await SetAsync(key, result, cacheTime, cancellationToken);

        return result;
    }

    public async Task SetAsync(string key, object? data, int? cacheTime = null, CancellationToken cancellationToken = default)
    {
        if (data is not null)
        {
            //serialize item
            var serializedItem = JsonSerializer.Serialize(data);
            if (cacheTime is not null || _redisOptions.CacheTime > 0)
            {
                //set cache time
                var expiresIn = TimeSpan.FromMinutes(cacheTime ?? _redisOptions.CacheTime);
                var options = new DistributedCacheEntryOptions()
                       .SetSlidingExpiration(expiresIn);
                await _distributedCache.SetStringAsync(key, serializedItem, options, cancellationToken);
            }
            else
                await _distributedCache.SetStringAsync(key, serializedItem, cancellationToken);

            CacheKeys.TryAdd(key, false);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        //we should always persist the data protection key list
        if (!key.Equals(_redisOptions.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            CacheKeys.TryRemove(key, out bool _);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }
}
