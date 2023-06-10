using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Json;
using CasseroleX.Application.Configurations;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebUI.Filters;
using WebUI.OptionSetup;
using WebUI.Services;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All)); //HTML乱码问题
        //启用Session
        services.AddSession(options =>
        {
            //配置了redis 这里session 存在redis中

            //options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.Name = "MySessionCookie"; // 设置会话Cookie的名称 
            options.Cookie.HttpOnly = true; //指示客户端脚本是否可以访问 Cookie
           //options.Cookie.IsEssential = false;//指示此 Cookie 是否对于应用程序正常运行至关重要。 如果为 true，则可能会绕过同意策略检查。 默认值为 false
        });
        //站点设置
        services.ConfigureOptions<AppOptionsSetup>();

        //服务注入 
        services.AddScoped<ICustomAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IAdminManager, AdminManager>(); 
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IRoleManager, RoleManager>(); 
        services.AddScoped<ISiteConfigurationService, SiteConfigurationService>();

        services.AddHttpContextAccessor(); 

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();


        // 注册自定义视图返回对象
        services.TryAddSingleton<IActionResultExecutor<ViewResult>, CustomViewEngine>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<GlobalRequestFilter>(); //请求过滤器
            options.Filters.Add<GlobalExceptionFilter>();

        }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = new LowercaseNamingPolicy()); 
        services.AddFluentValidationClientsideAdapters();
        services.AddRazorPages();

        services.AddControllersWithViews(options =>options.Filters.Add(typeof(GlobalRequestFilter)))
        .AddJsonOptions(options =>
        { 
            options.JsonSerializerOptions.PropertyNamingPolicy = new LowercaseNamingPolicy();
            options.JsonSerializerOptions.DictionaryKeyPolicy = new LowercaseNamingPolicy();
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; //忽略循环引用
            options.JsonSerializerOptions.WriteIndented = true;
            
        }); 
        services.AddFluentValidationClientsideAdapters();
        services.AddRazorPages();


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        //services.AddOpenApiDocument(configure =>
        //{
        //    configure.Title = "CleanTest API";
        //    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
        //    {
        //        Type = OpenApiSecuritySchemeType.ApiKey,
        //        Name = "Authorization",
        //        In = OpenApiSecurityApiKeyLocation.Header,
        //        Description = "Type into the textbox: Bearer {your JWT token}."
        //    });

        //    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        //});

        return services;
    }
}
