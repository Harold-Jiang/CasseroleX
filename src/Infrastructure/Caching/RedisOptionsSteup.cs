using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.Caching;

public class RedisOptionsSetup : IConfigureOptions<RedisOptions>
{
    private const string _sectionName = "RedisOptions";
    private readonly IConfiguration _configuration;

    public RedisOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RedisOptions options)
    {
        _configuration.GetSection(_sectionName).Bind(options);
    }
}
