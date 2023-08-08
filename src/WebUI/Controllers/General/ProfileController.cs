using CasseroleX.Application.Admins.Commands.UploadProfile;
using CasseroleX.Application.Admins.Queries;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.General;

[Route("general/profile")]
public class ProfileController : BaseAdminController
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
        var admin = await Mediator.Send(new GetAdminDetailQuery(User.Identity.ID()));
        return View(admin);
    }

    [HttpPost("update")]
    public async Task<Result> UpdateAsync(UploadProfileCommand command)
    { 
        return await Mediator.Send(command);
    }
}
