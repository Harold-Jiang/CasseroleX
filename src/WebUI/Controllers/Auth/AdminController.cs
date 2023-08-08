using CasseroleX.Application.Admins.Commands.CreateAdmin;
using CasseroleX.Application.Admins.Commands.DeleteAdmin;
using CasseroleX.Application.Admins.Commands.UpdateAdmin;
using CasseroleX.Application.Admins.Queries;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

/// <summary>
/// admin Management
/// </summary>
[Route("Auth/Admin")] 
public class AdminController : BaseAdminController
{
     
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync(SearchQuery<Admin, AdminDto> query)
    {
        if (!User.Identity.IsSuperAdmin()) {
            throw new PermissionException("Access is allowed only to the super management group");
        }
        if (IsAjaxRequest())
        {
            var result = await Mediator.Send(query);
            return Json(new { Total = result.TotalCount, Rows = result.Items });
        }
        ViewBag.JsConfig.Add("admin", new { Id = User.Identity.ID() });
        return View();

    }

    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateAdminCommand command)
    {
        return await Mediator.Send(command);
    }

   
    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateAdminCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var admin = await Mediator.Send(new GetAdminDetailQuery(ids.ToInt()));
        return View(admin);
    }

    [HttpPost("del/ids/{ids}")]
    public async Task<Result> DeleteAsync(string ids)
    {
        return await Mediator.Send(new DeleteAdminCommand(ids)); 
    }
}
