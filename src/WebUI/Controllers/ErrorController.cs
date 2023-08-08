using CasseroleX.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class ErrorController : Controller
{ 
    public IActionResult NoFound(int statusCode)
    {
        //var result = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        return statusCode switch
        {
            404 => View("Jump", Result.Failure(OperationResult.PAGE_NOTFOUND, url: Url.Action("Index", "Index"))),
            500 => View("Jump", Result.Failure(OperationResult.SERVER_EXCEPTION, url: Url.Action("Index", "Index"))),
            _ => View("Jump", Result.Failure(OperationResult.SERVER_EXCEPTION, url: Url.Action("Index", "Index"))),
        };
    }

    public IActionResult NoAuthorize(string query)
    { 
        return View("Error", Result.Failure(OperationResult.NO_PERMISSION, url: query));
    }

}
