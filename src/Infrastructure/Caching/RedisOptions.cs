namespace CasseroleX.Infrastructure.Caching;

public class RedisOptions
{
    public string? RedisDataProtectionKey { get; set; }
    public int CacheTime { get; set; }
    public string? RedisConnectionString { get; set; }
    public int RedisDatabaseId { get; set; }

}
