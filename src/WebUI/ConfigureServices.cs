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
using WebUI.Services;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All)); //HTML Unicode

        //Session
        services.AddSession(options =>
        {
            //options.IdleTimeout = TimeSpan.FromSeconds(10);
            options.Cookie.Name = "CasserolexSession"; 
            options.Cookie.HttpOnly = true; 
     
        });
 
         
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICustomAuthenticationService, CookieAuthenticationService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IAdminManager, AdminManager>(); 
        services.AddScoped<IRoleManager, RoleManager>(); 
        services.AddScoped<ISiteConfigurationService, SiteConfigurationService>();



        services.AddHttpContextAccessor(); 

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // Custom ViewResult Object
        services.TryAddSingleton<IActionResultExecutor<ViewResult>, CustomViewEngine>();
        
        services.AddFluentValidationClientsideAdapters();
        services.AddRazorPages();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<GlobalRequestFilter>(); 
            options.Filters.Add<GlobalExceptionFilter>(); 
            options.Conventions.Add(new CustomRouteConvention());
        })
        .AddJsonOptions(options =>
        { 
            options.JsonSerializerOptions.PropertyNamingPolicy = new LowercaseNamingPolicy();
            options.JsonSerializerOptions.DictionaryKeyPolicy = new LowercaseNamingPolicy();
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
            options.JsonSerializerOptions.WriteIndented = true; 

        }); 
        services.AddFluentValidationClientsideAdapters();
        services.AddRazorPages();

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = false;
        });

      
        return services;
    }
}
