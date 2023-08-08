using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Infrastructure.OptionSetup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace WebUI.Controllers;

public class IndexController : BaseAdminController
{
    private readonly AppOptions _app;
    private readonly ICurrentUserService _currentUserService;

    public IndexController(IOptionsSnapshot<AppOptions> app,
        ICurrentUserService currentUserService)
    {
        _app = app.Value;
        _currentUserService = currentUserService;
    }
     
    public async Task<IActionResult> IndexAsync()
    {
        Dictionary<string, string> cookieArr = new()
        {
            { "adminskin", @"^skin\-([a-z\-]+)$" },
            { "multiplenav", @"^(0|1)$" },
            { "multipletab", @"^(0|1)$" },
            { "show_submenu", @"^(0|1)$" },
            { "sidebar_collapse", @"^(0|1)$" },
        };

        foreach (var item in cookieArr)
        {
            HttpContext.Request.Cookies.TryGetValue(item.Key, out string? cookieValue);
            if (!string.IsNullOrEmpty(cookieValue) && cookieValue.preg_match(item.Value))
            {
                switch (item.Key)
                {
                    case "adminskin":
                        _app.AdminSkin = cookieValue;
                        break;
                    case "multiplenav":
                        _app.MultipleNav = cookieValue == "1" ? true : false;
                        break;
                    case "multipletab":
                        _app.MultipleTab = cookieValue == "1" ? true : false;
                        break;
                    case "show_submenu":
                        _app.ShowSubMenu = cookieValue == "1" ? true : false;
                        break;
                    case "sidebar_collapse":
                        ViewBag.SidebarCollapse = cookieValue;
                        break;
                }
            }
        }
          
        var sidebar = await Mediator.Send(new GetHomeMenusQuery
        { 
            MultipleNav = _app.MultipleNav,
            ShowSubMenu = _app.ShowSubMenu,
            RefererUrl = HttpContext.Session.GetString(HeaderNames.Referer),
        });

        if (IsPost() && Request.Form["action"] == "refreshmenu")
        {
            return Json(Result.Success(data: new { menulist = sidebar.MenuList, navlist = sidebar.NavList }, url: "/index/index"));
        }
        return View(sidebar);
    }
}