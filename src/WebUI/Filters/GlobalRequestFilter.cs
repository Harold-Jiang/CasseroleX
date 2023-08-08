using System.Security.Claims;
using System.Text.RegularExpressions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Infrastructure.OptionSetup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using WebUI.Helpers;

namespace WebUI.Filters;

public class GlobalRequestFilter : IAsyncActionFilter
{
    private readonly ISiteConfigurationService _siteConfigurationService;
    private readonly IApplicationDbContext _context;
    private readonly AppOptions _app;

    public GlobalRequestFilter(ISiteConfigurationService siteConfigurationService,
        IOptionsSnapshot<AppOptions> app,
        IApplicationDbContext context)
    {
        _siteConfigurationService = siteConfigurationService;
        _app = app.Value;
        _context = context;
    }
    /// <summary>
    /// Before entering the controller
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Redirect when not a tab
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
        // Generate Jsconfg
        if (HttpMethods.IsGet(context.HttpContext.Request.Method))
        {
            if (context.Controller is Controller controller)
            {
                var jsconfig = await InitializeJsConfig(context.HttpContext);
                controller.ViewBag.JsConfig = jsconfig;
            }
        }
        await next();

        //admin log
        if (HttpMethods.IsPost(context.HttpContext.Request.Method))
        {
            var title = WebTools.GetControllerName(context.HttpContext);
            if (title.IsNotNullOrEmpty())
            {
                var userName = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userName != null)
                {
                    AdminLog model = new()
                    {
                        AdminId = int.Parse(context.HttpContext.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                        UserName = userName,
                        CreateTime = DateTime.Now,
                        Ip = WebTools.GetIpAddress(context.HttpContext),
                        UserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString(),
                        Url = context.HttpContext.Request.Path.ToString(),
                        Title = $"{title}/{WebTools.GetActionName(context.HttpContext)}",
                        Content = $"Parameter:{{Query:\"{context.HttpContext.Request.QueryString}\"}},{{From:{{{GetFromString(context.HttpContext.Request.Form)}}}}}"
                    };
                    await _context.AdminLogs.AddAsync(model);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
     

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
        var sysconfig = await _siteConfigurationService.GetConfigurationAsync<SystemConfigInfo>();
        var upload = await _siteConfigurationService.GetConfigurationAsync<UploadConfigInfo>();

        var controllerName = WebTools.GetControllerName(context).ToLowerInvariant();
        var actionName = WebTools.GetActionName(context).ToLowerInvariant();
         
        Dictionary<string, object> jsConfig = new()
        {
            { "site", new { name = sysconfig.Name, version = sysconfig.Version, cdnurl = sysconfig.CDN,apiurl="", timezone = sysconfig.Timezone, languages = new { backend = sysconfig.Language } } },
            { "upload", upload },
            { "modulename", "admin" },
            { "controllername", controllerName.Replace("/", ".") },
            { "actionname", actionName },
            { "jsname", $"backend/{controllerName}" },
            { "moduleurl", "" },
            { "language", sysconfig.Language },
            { "referer", WebTools.GetCustomReferrer(context)??"" },
            { "__PUBLIC__", sysconfig.PUBLIC },
            { "__ROOT__", "" },
            { "__CDN__", sysconfig.CDN },
            { "cookie", new { prefix = sysconfig.Prefix } }
        };
        return jsConfig;
    }

    /// <summary>
    /// get parameter
    /// </summary> 
    private static string GetFromString(IFormCollection Form)
    {
        string req = String.Empty;
        foreach (var item in Form.Keys)
        {
            req += "," + item;
            if (!StringValues.IsNullOrEmpty(Form[item]) && item.preg_match(@"(password|salt|token|__RequestVerificationToken)"))
            {
                req += ":***";
            }
            else
            {
                req += $":\"{Form[item]}\"";
            }
        }
        return req.TrimStart(',');
    }
}
