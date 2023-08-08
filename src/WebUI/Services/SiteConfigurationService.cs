using System.Reflection;
using System.Text.Json;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services;

public class SiteConfigurationService : ISiteConfigurationService
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public SiteConfigurationService(IApplicationDbContext context, 
        ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<T> GetConfigurationAsync<T>() where T : IConfig,new()
    {
        return await _cache.GetAsync<T>(string.Format(CacheKeys.CONFIGURATION_SITE_BY_TYPE_KEY, typeof(T).Name.ToLowerInvariant()),
        async () =>
        {
            var configs = await _context.SiteConfigurations.ToArrayAsync();
            if (configs is not null)
            { 
                return GetConfigInstance<T>(Activator.CreateInstance<T>(), configs) ?? new();
            }
            return new();
        }) ?? new();
         
    }

    /// <summary>
    /// Set the values of the configuration model and return the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="sysconfigs"></param>
    /// <returns></returns>
    private static T GetConfigInstance<T>(T obj, SiteConfiguration[] sysconfigs)
    {
        var type = typeof(T);
        foreach (var item in type.GetProperties())
        {
            var prop = sysconfigs.SingleOrDefault(s => s.Name.ToLowerInvariant() == item.Name.ToLowerInvariant());
            if (prop != null && prop.Value.IsNotNullOrEmpty())
            {
                var propertyInfo = type.GetProperty(item.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                // If the corresponding attribute is found in T
                if (propertyInfo != null)
                { 
                    Type realType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;  
                    try
                    {
                        //Judgment type
                        if (realType == typeof(Dictionary<string, string>))
                        {
                            var propertyValue = JsonSerializer.Deserialize<Dictionary<string, string>>(prop.Value!);
                            if (propertyValue != null)
                                propertyInfo.SetValue(obj, propertyValue);
                        }
                        else if (realType == typeof(bool)) 
                        {
                            bool propertyValue = prop.Value == "1";
                            propertyInfo.SetValue(obj, propertyValue);
                        }
                        else
                        {
                            var propertyValue = Convert.ChangeType(prop.Value, realType);
                            propertyInfo.SetValue(obj, propertyValue);
                        }
                    }
                    catch
                    {
                        throw new Exception($"Configuration option assignment error: Field {item.Name} Value {prop.Value} Type {realType}}}");
                    }
                }
            }
        }
        return obj;
    }
}
