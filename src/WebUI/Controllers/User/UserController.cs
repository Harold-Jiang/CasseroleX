using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Users.Commands.CreateUser;
using CasseroleX.Application.Users.Commands.DeleteUser;
using CasseroleX.Application.Users.Commands.UpdateUser;
using CasseroleX.Application.Users.Queries;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

/// <summary>
/// User Management
/// </summary>
[Route("User/User")] 
public class UserController : BaseAdminController
{
     
    [HttpGet]
    [HttpGet("index")]
    public async Task<IActionResult> IndexAsync(SearchQuery<CasseroleX.Domain.Entities.User,UserDto> query)
    {
        if (IsAjaxRequest())
        {
            var result = await Mediator.Send(query);
            return Json(new { Total = result.TotalCount, Rows = result.Items });
        }
        return View();
    }

    [HttpPost("add")]
    public async Task<Result> AddAsync(CreateUserCommand command)
    {
        return await Mediator.Send(command);
    }

   
    [HttpGet("edit/ids/{ids}")]
    [HttpPost("edit/ids/{ids}")]
    public async Task<IActionResult> EditAsync(string ids, UpdateUserCommand command)
    {
        if (IsPost())
        {
            command.Id = ids.ToInt();
            return Json(await Mediator.Send(command));
        }
        var admin = await Mediator.Send(new GetUserDetailQuery(ids.ToInt()));
        return View(admin);
    }

    [HttpPost("del/ids/{ids}")]
    public async Task<Result> DeleteAsync(string ids)
    {
        return await Mediator.Send(new DeleteUserCommand(ids)); 
    }
}
