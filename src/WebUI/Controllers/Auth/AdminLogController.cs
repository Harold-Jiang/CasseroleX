using CasseroleX.Application.Admins.Commands.DeleteAdminLog;
using CasseroleX.Application.Admins.Queries;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

[Route("auth/adminlog")]
public class AdminLogController : BaseAdminController
{
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync(SearchQuery<AdminLog, AdminLog> query)
    {
        if (IsAjaxRequest())
        {
            var result = await Mediator.Send(query);
            return Json(new { Total = result.TotalCount, Rows = result.Items });
        }
        return View();
    }

    [HttpGet("detail/ids/{ids}")] 
    public async Task<IActionResult> DetailAsync([FromRoute] int ids)
    {
        var admin = await Mediator.Send(new GetAdminLogDetailQuery(ids.ToInt()));
        return View(admin);

    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteAdminLogCommand command)
    {
        return await Mediator.Send(command);
    }

}
