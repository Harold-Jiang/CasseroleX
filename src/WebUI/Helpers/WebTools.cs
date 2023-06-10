using System.Net;

namespace WebUI.Helpers;
public static class WebTools
{
    public static string GetIpAddress(HttpContext? context)
    {
        if (context == null) return string.Empty;

        var remoteIpAddress = context.Connection.RemoteIpAddress;

        // 如果是IPv4地址，则直接返回
        if (remoteIpAddress is not null && remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            return remoteIpAddress.ToString();
        }

        // 否则，尝试获取IPv4地址
        var forwardedHeader = context.Request.Headers["X-Forwarded-For"];
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            var ips = forwardedHeader.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(s => s.Trim())
                                     .ToList();

            // 从前往后遍历，找到第一个IPv4地址
            foreach (var ip in ips)
            {
                if (IPAddress.TryParse(ip, out var address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }
        }

        // 如果无法获取IPv4地址，则返回IPv6地址
        return remoteIpAddress is null? string.Empty : remoteIpAddress.ToString();
    }
}
