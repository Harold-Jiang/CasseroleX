using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace WebUI.Helpers;
public static class WebTools
{
    /// <summary>
    /// Get Ipaddress
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetIpAddress(HttpContext? context)
    {
        if (context == null) return string.Empty;

        var remoteIpAddress = context.Connection.RemoteIpAddress;

        if (remoteIpAddress is not null && remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return remoteIpAddress.ToString();
        }

        var forwardedHeader = context.Request.Headers["X-Forwarded-For"];
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            var ips = forwardedHeader.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => s.Trim())
                                     .ToList();

            foreach (var ip in ips)
            {
                if (IPAddress.TryParse(ip, out var address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
        }

        return remoteIpAddress is null? string.Empty : remoteIpAddress.ToString();
    }

    /// <summary>
    /// Get Controller Name
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetControllerName(HttpContext context)
    {
        var routeAttribute = context!.GetEndpoint()?.Metadata.OfType<RouteAttribute>().SingleOrDefault();
        string? controllerName;
        if (routeAttribute != null)
            controllerName = routeAttribute.Template;
        else
            controllerName = context!.Request.RouteValues["controller"]?.ToString()?.ToLowerInvariant();
        return controllerName ?? "";
    }

    /// <summary>
    /// Get Action Name
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetActionName(HttpContext context)
    {
        return context.Request.RouteValues == null ? "" :
            context.Request.RouteValues["action"]?.ToString() ?? "";
    }

    /// <summary>
    /// Get Referrer Url
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string? GetCustomReferrer(HttpContext context)
    {
        return context!.Session.GetString(HeaderNames.Referer);
    }
}
