using CasseroleX.Application.DashBoard.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

/// <summary>
/// Dashboard
/// </summary>
public class DashboardController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        var result = await Mediator.Send(new GetDashBoardQuery()); 
        ViewBag.JsConfig.Add("column", result.UserList?.Keys);
        ViewBag.JsConfig.Add("userdata", result.UserList?.Values);
        return View(result);
    }
}

