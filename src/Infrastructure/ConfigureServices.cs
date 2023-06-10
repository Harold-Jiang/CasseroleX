using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Authorization;
using CasseroleX.Infrastructure.Caching;
using CasseroleX.Infrastructure.Files;
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
         
        //身份验证
        services.AddCookieAuthentication(configuration);

        //授权服务
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<ICustomAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<ICsvFileBuilder, CsvFileBuilder>();

        //缓存注入
        services.ConfigureOptions<RedisOptionsSetup>();
        services.AddStackExchangeRedisCache(options => options.Configuration = $"{configuration.GetSection("RedisOptions:RedisConnectionString").Value},defaultDatabase={configuration.GetSection("RedisOptions:RedisDatabaseId").Value}");
        services.AddSingleton<ICacheService, CacheService>();

        //安全设置注入
        services.ConfigureOptions<SecurityOptionsSetup>();

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

        ////add external authentication
        //authenticationBuilder.AddCookie(CookieAuthenticationDefaults.ExternalAuthenticationScheme, options =>
        //{
        //    options.Cookie.Name = securityConfig.CookiePrefix +
        //                          CookieAuthenticationDefaults.ExternalAuthenticationScheme;
        //    options.Cookie.HttpOnly = true;
        //    options.LoginPath = CookieAuthenticationDefaults.LoginPath;
        //    options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
        //    options.Cookie.SecurePolicy = securityConfig.CookieSecurePolicyAlways
        //        ? CookieSecurePolicy.Always
        //        : CookieSecurePolicy.SameAsRequest;
        //});

        ////register external authentication plugins now
        //var typeSearcher = new TypeSearcher();
        //var externalAuthConfigurations = typeSearcher.ClassesOfType<IAuthenticationBuilder>();
        //var externalAuthInstances = externalAuthConfigurations
        //.Where(PluginExtensions.OnlyInstalledPlugins)
        //    .Select(x => (IAuthenticationBuilder)Activator.CreateInstance(x))
        //    .OrderBy(x => x!.Priority);

        ////add new Authentication
        //foreach (var instance in externalAuthInstances)
        //    instance.AddAuthentication(authenticationBuilder, configuration);
    }

}
