using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[AutoValidateAntiforgeryToken] 
[HasPermission]
public class BaseAdminController : Controller
{
    private ISender? _mediator; 
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    [HttpGet("add")]
    public virtual IActionResult Add()
    {
        return View();
    }
     
    #region NoAction
    [NonAction]
    public virtual bool IsAjaxRequest()
    {
        if (HttpContext!.Request == null)
            throw new ArgumentNullException(nameof(HttpContext.Request));

        if (HttpContext.Request.Headers == null)
            return false;

        return HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    [NonAction]
    public virtual bool IsPost()
    {
        if (HttpContext!.Request == null)
            throw new ArgumentNullException(nameof(HttpContext.Request));

        return HttpMethods.IsPost(HttpContext!.Request.Method);
    }
    #endregion
}
