using CasseroleX.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

/// <summary>
/// 控制台
/// </summary>
public class DashboardController : BaseAdminController
{
    public DashboardController() 
    {
    }

    [HasPermission("dashboard/index")] 
    public IActionResult Index()
    {
        //CreateJsConfig();
        return View();
    }
}

