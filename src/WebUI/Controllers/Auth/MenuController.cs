using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Menus.Commands.CreateMenuCommand;
using CasseroleX.Application.Menus.Commands.DeleteMenuCommand;
using CasseroleX.Application.Menus.Commands.UpdateMenuCommand;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Public.Commands;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

/// <summary>
/// Menu Management
/// </summary>
[Route("Auth/Menu")] 
public class MenuController : BaseAdminController
{
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync()
    {
        if (IsAjaxRequest())
        {
            var menus = await Mediator.Send(new GetMenusQuery());
            return new JsonResult(new { Total = menus.Count, Rows = menus });
        }
        return View(); 
    }


    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateMenuCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateMenuCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var admin = await Mediator.Send(new GetMenuDetailQuery(ids.ToInt()));
        return View(admin);
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteMenuCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPost("multi")]
    public async Task<Result> MultiAsync(MultiCommand<RolePermissions> command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("sort")]
    public async Task<Result> SortAsync(SortCommand<RolePermissions> command)
    {
        return await Mediator.Send(command);
    }
}
