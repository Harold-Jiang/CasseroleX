using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Authorization;
using CasseroleX.Infrastructure.Caching;
using CasseroleX.Infrastructure.Files;
using CasseroleX.Infrastructure.OptionSetup;
using CasseroleX.Infrastructure.Persistence;
using CasseroleX.Infrastructure.Persistence.Interceptors;
using CasseroleX.Infrastructure.Security;
using CasseroleX.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        var _connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString)));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        //Authentication
        services.AddCookieAuthentication(configuration);

        //Authorization
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<ICustomAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<ICsvFileBuilder, CsvFileBuilder>();
        services.AddScoped<IUploadService, UploadService>();

        //Email and sms
        services.AddScoped<IEmailSender, MessageServices>();
        services.AddScoped<ISmsSender, MessageServices>();

        //Cache
        services.ConfigureOptions<RedisOptionsSetup>();
        if (configuration.GetValue<bool>("RedisOptions:UseRedisCache"))
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = $"{configuration.GetSection("RedisOptions:RedisConnectionString").Value},defaultDatabase={configuration.GetSection("RedisOptions:RedisDatabaseId").Value}");
        }
        else
        {
            services.AddDistributedMemoryCache();
        }
        services.AddSingleton<ICacheService, CacheService>();

        //Security
        services.ConfigureOptions<SecurityOptionsSetup>();
        //App setting
        services.ConfigureOptions<AppOptionsSetup>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        return services;
    }

    public static void AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var securityConfig = new SecurityOptions();
        configuration.GetSection("SecurityOptions").Bind(securityConfig);

        //set default authentication schemes
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });

        //add main cookie authentication
        authenticationBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name =
                securityConfig.CookiePrefix + CookieAuthenticationDefaults.AuthenticationScheme;
            options.Cookie.HttpOnly = true;
            options.LoginPath = CookieAuthenticationDefaults.LoginPath;
            options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;

            options.Cookie.SecurePolicy = securityConfig.CookieSecurePolicyAlways
                ? CookieSecurePolicy.Always
                : CookieSecurePolicy.SameAsRequest;
        });

   
    }

}
