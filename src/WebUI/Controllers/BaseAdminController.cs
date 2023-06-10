using CasseroleX.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class BaseAdminController : Controller
{
    private ISender? _mediator; 
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
  
    /// <summary>
    /// 获取请求是否是用AJAX进行的
    /// </summary>
    /// <param name="request">HTTP请求</param>
    /// <returns>Result </returns>
    public virtual bool IsAjaxRequest()
    {
        if (HttpContext!.Request == null)
            throw new ArgumentNullException(nameof(HttpContext.Request));

        if (HttpContext.Request.Headers == null)
            return false;

        return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    /// <summary>
    /// 是否是Post请求
    /// </summary>
    /// <param name="request">HTTP请求</param>
    /// <returns>Result </returns>
    public virtual bool IsPost()
    {
        if (HttpContext!.Request == null)
            throw new ArgumentNullException(nameof(HttpContext.Request));

        return HttpMethods.IsPost(HttpContext!.Request.Method);
    }

    /// <summary>
    /// 是否是Get请求
    /// </summary>
    /// <param name="request">HTTP请求</param>
    /// <returns>Result </returns>
    public virtual bool IsGet()
    {
        if (HttpContext!.Request == null)
            throw new ArgumentNullException(nameof(HttpContext.Request));

        return HttpMethods.IsGet(HttpContext!.Request.Method);
    }


    /// <summary>
    /// 获取请求是否Dialog 
    /// </summary>
    /// <param name="request">HTTP请求</param>
    /// <returns>Result </returns>
    public virtual bool IsDialogRequest()
    {
        return HttpContext!.Request == null
            ? throw new ArgumentNullException(nameof(HttpContext.Request))
            : !string.IsNullOrEmpty(HttpContext!.Request.Query["dialog"].ToString());
    }

}
