using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.Security;

public class SecurityOptionsSetup : IConfigureOptions<SecurityOptions>
{
    private const string _sectionName = "SecurityOptions";
    private readonly IConfiguration _configuration;

    public SecurityOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SecurityOptions options)
    {
        _configuration.GetSection(_sectionName).Bind(options);
    }
}
