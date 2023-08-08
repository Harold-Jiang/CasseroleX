using CasseroleX.Application.Common.Models;
using CasseroleX.Application.UserGroups.Commands.CreateUserGroup;
using CasseroleX.Application.UserGroups.Commands.DeleteUserGroup;
using CasseroleX.Application.UserGroups.Commands.UpdateUserGroup;
using CasseroleX.Application.UserGroups.Queries;
using CasseroleX.Application.UserRules.Queries;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.User;

[Route("User/Group")]
public class UserGroupController : BaseAdminController
{
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync()
    {
        if (IsAjaxRequest())
        {
            var groups = await Mediator.Send(new GetUserGroupsQuery());
            return new JsonResult(new { Total = groups.Count, Rows = groups });
        }
        return View();
    }

    

    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateUserGroupCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateUserGroupCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var group = await Mediator.Send(new GetUserGroupDetailQuery(ids.ToInt()));
        ViewBag.NodeList = await Mediator.Send(new GetRuleTreeQuery(group?.Rules.ToIList<int>()));
        return View(group);
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteUserGroupCommand command)
    {
        return await Mediator.Send(command);
    }
}
