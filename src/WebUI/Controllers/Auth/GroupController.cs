using AutoMapper;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Roles.Commands.CreateRole;
using CasseroleX.Application.Roles.Commands.DeleteRole;
using CasseroleX.Application.Roles.Commands.UpdateRole;
using CasseroleX.Application.Roles.Queries;
using CasseroleX.Infrastructure.Authentication;
using CasseroleX.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Auth;

/// <summary>
/// 角色管理
/// </summary>
[Route("Auth/Group")]
public class GroupController : BaseAdminController
{
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GroupController(IRoleManager roleManager, IApplicationDbContext context, IMapper mapper)
    {
        _roleManager = roleManager;
        _context = context;
        _mapper = mapper;


    }


    [HttpGet]
    [HttpGet("index")]
    [HasPermission("auth/group/index")]
    public async Task<IActionResult> Index()
    {
        //var roleName = new Dictionary<int, string>();
        if (IsAjaxRequest())
        {
            var roles = await Mediator.Send(new GetRoleGroupsQuery
            {
                UserId = User.Identity.ID(),
                IsSuperAdmin = User.Identity.IsSuperAdmin()
            });
            //foreach (var role in roles)
            //{
            //    roleName[role.Id] = role.Name ?? "";
            //}
            return new JsonResult(new { Total = roles.Count, Rows = roles });
        }
        ViewBag.JsConfig.Add("admin", new { Id = User.Identity.ID(), group_ids = User.Identity.GetRoleIds() });
        //ViewBag.JsConfig.Add("groupdata", roleName);
        return View();
    }

    [HttpGet("add")]
    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(CreateRoleCommand command)
    {
        if (IsPost())
        {
            await Mediator.Send(command);
            return Json(Result.Success());
        }
        return View();
    }

    [HttpGet("edit")]
    [HttpPost("edit")]
    public async Task<ActionResult> Update(int id, UpdateRoleCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteRoleCommand(id));

        return NoContent();
    }
}
