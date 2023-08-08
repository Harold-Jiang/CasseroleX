using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Public.Commands;
using CasseroleX.Application.UserRules.Commands.CreateUserRule;
using CasseroleX.Application.UserRules.Commands.DeleteUserRule;
using CasseroleX.Application.UserRules.Commands.UpdateUserRule;
using CasseroleX.Application.UserRules.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.User;

[Route("User/Rule")]
public class UserRuleController : BaseAdminController
{
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync()
    {
        if (IsAjaxRequest())
        {
            var rules = await Mediator.Send(new GetUserRulesQuery());
            return new JsonResult(new { Total = rules.Count, Rows = rules });
        }
        return View();
    }

    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateUserRuleCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateUserRuleCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var rule = await Mediator.Send(new GetUserRuleDetailQuery(ids.ToInt()));
        return View(rule);
    }

    [HttpPost("del")]
    public async Task<Result> DeleteAsync(DeleteUserRuleCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("multi")]
    public async Task<Result> MultiAsync(MultiCommand<UserRule> command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("sort")]
    public async Task<Result> SortAsync(SortCommand<UserRule> command)
    {
        return await Mediator.Send(command);
    }
}
