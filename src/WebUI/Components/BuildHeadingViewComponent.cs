using CasseroleX.Application.Common.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class BuildHeadingViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context;
    public BuildHeadingViewComponent(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? path = null, bool container = true)
    {
        var html = await GenerateHeadingAsync(path, container);
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
    public async Task<string> GenerateHeadingAsync(string? path = null, bool container = true)
    {
        string title = string.Empty;
        string content = string.Empty;

        if (string.IsNullOrEmpty(path))
        {
            string action = Request.Query["action"].ToString();
            string controller = Request.Query["controller"].ToString().Replace(".", "/");
            path = controller.ToLower() + (action != null && action != "index" ? "/" + action : "");
        }

        // 根据当前的URI自动匹配父节点的标题和备注
        var data = await _context.RolePermissions.FirstOrDefaultAsync(x=>x.Name == path);
        if (data != null)
        {
            title = data.Title??"";
            content = data.Remark??"";
        }

        if (string.IsNullOrEmpty(content))
        {
            return string.Empty;
        }

        string result = "<div class=\"panel-lead\"><em>" + title + "</em>" + content + "</div>";
        if (container)
        {
            result = "<div class=\"panel-heading\">" + result + "</div>";
        }

        return result;
    }
     
}
