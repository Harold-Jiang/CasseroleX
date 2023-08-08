using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Roles.Commands.CreateRole;
using CasseroleX.Application.Roles.Commands.DeleteRole;
using CasseroleX.Application.Roles.Commands.UpdateRole;
using CasseroleX.Application.Roles.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

/// <summary>
/// Role Management
/// </summary>
[Route("Auth/Role")] 
public class RoleController : BaseAdminController
{
  
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync()
    {
        if (IsAjaxRequest())
        {
            var roles = await Mediator.Send(new GetRolesQuery());
            return new JsonResult(new { Total = roles.Count, Rows = roles });
        }
        ViewBag.JsConfig.Add("admin", new { Id = User.Identity.ID(), group_ids = User.Identity.GetRoleIds() });
        return View();
    }
     
    [HttpPost("roletree")]
    [IgnoreAntiforgeryToken]
    public async Task<Result> RoleTree(GetRoleTreeQuery  query)
    {
        var result = await Mediator.Send(query); 
        return Result.Success(result);
    }

    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateRoleCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateRoleCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var admin = await Mediator.Send(new GetRoleDetailQuery(ids.ToInt()));
        return View(admin);
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteRoleCommand command)
    {
        return await Mediator.Send(command);
    }
}
