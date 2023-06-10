using System.Text.RegularExpressions;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebUI.OptionSetup;

namespace WebUI.Filters;

public class GlobalRequestFilter : IAsyncActionFilter
{
    private readonly ISiteConfigurationService _siteConfigurationService;
    private readonly AppOptions _app;

    public GlobalRequestFilter(ISiteConfigurationService siteConfigurationService,
        IOptionsSnapshot<AppOptions> app)
    {
        _app = app.Value;
        _siteConfigurationService = siteConfigurationService;
    }
    /// <summary>
    /// 进入控制器前
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 非选项卡时重定向
        if (CheckRequst(context))
        {
            string url = $"{context.HttpContext!.Request.Path}{context.HttpContext.Request.QueryString}";
            string str = @"([\?|&]+)ref=addtabs(&?)";
            Regex re = new(str, RegexOptions.IgnoreCase);
            var matchs = re.Matches(url);
            if (matchs.Count > 0)
            {
                url = matchs[0].Groups[2].Value == "&" ? url.preg_replace(new string[] { str }, new string[] { matchs[0].Groups[1].Value }) : url.preg_replace(new string[] { str }, new string[] { "" });
            }
            context.Result = new RedirectResult($"/{context.HttpContext.Request.PathBase}", false);
            context.HttpContext.Session.SetString(HeaderNames.Referer, url);
            return;
        }
        //生成Jsconfg
        if (HttpMethods.IsGet(context.HttpContext.Request.Method))
        {
            if (context.Controller is Controller controller)
            {
                var jsconfig = await InitializeJsConfig(context.HttpContext);
                controller.ViewBag.JsConfig = jsconfig;
            }
        }
        await next();
    }
     
    #region 方法
    
    private static bool CheckRequst(ActionExecutingContext context)
    {
        return !HttpMethods.IsPost(context.HttpContext.Request.Method) &&
            context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest" &&
            string.IsNullOrEmpty(context.HttpContext!.Request.Query["dialog"]) &&
            string.IsNullOrEmpty(context.HttpContext!.Request.Query["addtabs"]) &&
            context.HttpContext!.Request.Query["ref"].ToString() == "addtabs";
    }
    private async Task<Dictionary<string, object>> InitializeJsConfig(HttpContext context)
    {
        var sysconfig = await _siteConfigurationService.GetConfiguration<SystemConfigInfo>();
        var upload = await _siteConfigurationService.GetConfiguration<UploadConfigInfo>();

        var controllerName = GetControllerName(context).ToLowerInvariant();
        var actionName = GetActionName(context);
        Dictionary<string, object> jsConfig = new()
        {
            { "site", new { name = sysconfig.AppName, version = _app.Version, cdnurl = _app.CDN,apiurl="", timezone = "Asia/Shanghai", languages = new { backend = "zh-cn", frontend = "zh-cn" } } },
            { "upload", upload },
            { "modulename", "admin" },
            { "controllername", controllerName.Replace("/", ".") },
            { "actionname", actionName },
            { "jsname", "backend/" + (controllerName.Contains("account") ? "account" : controllerName) },
            { "moduleurl", "" },
            { "language", "zh-cn" },
            { "referer", GetCustomReferrer(context)??"" },
            { "__PUBLIC__", _app.PUBLIC },
            { "__ROOT__", _app.ROOT },
            { "__CDN__", _app.CDN },
            { "cookie", new { prefix = _app.Prefix } }
        };
        return jsConfig;
    }

    /// <summary>
    /// 获取Controller Name
    /// </summary> 
    private static string GetControllerName(HttpContext context)
    {
        var routeAttribute = context!.GetEndpoint()?.Metadata.OfType<RouteAttribute>().SingleOrDefault();
        string? controllerName;
        if (routeAttribute != null)
            controllerName = routeAttribute.Template;
        else
            controllerName = context!.Request.RouteValues["controller"]?.ToString()?.ToLower();
        return controllerName ?? "";
    } 
    /// <summary>
    /// 获取ActionName
    /// </summary> 
    private static string GetActionName(HttpContext context)
    {
        return context.Request.RouteValues == null ? "" :
            context.Request.RouteValues["action"]?.ToString()?.ToLower() ?? "";
    }

    /// <summary>
    /// 获取Session自定义 referrer
    /// </summary>
    private static string? GetCustomReferrer(HttpContext context)
    {
        return context!.Session.GetString(HeaderNames.Referer);
    }
    #endregion
}
