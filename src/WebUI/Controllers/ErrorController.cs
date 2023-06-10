using CasseroleX.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ErrorController : Controller
{
    

    /// <summary>
    /// 页面异常
    /// </summary>
    public IActionResult NoFound(int statusCode)
    {
        //var result = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        return statusCode switch
        {
            404 => View("Jump", Result.Failure("页面没有找到", url: Url.Action("Index", "Index"))),
            500 => View("Jump", Result.Failure("服务器异常", url: Url.Action("Index", "Index"))),
            _ => View("Jump", Result.Failure("服务器异常", url: Url.Action("Index", "Index"))),
        };
    }

    /// <summary>
    /// 没有权限
    /// </summary>
    /// <returns></returns>

    public IActionResult NoAuthorize(string query)
    {
         
        return View("Error", Result.Failure("您没有权限访问此页面", url: query));
    }

}
