using CasseroleX.Application.Common.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace WebUI.Components;


public class BuildToolbarViewComponent : ViewComponent
{
    private readonly ICurrentUserService _currentUserService;

    public BuildToolbarViewComponent(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string[] btns)
    {
        var html = await GenerateButtonsAsync(btns);
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }

    public async Task<string> GenerateButtonsAsync(string[] btns, Dictionary<string, string[]>? attr = null)
    {
        var controller = Request.Path.Value?.Substring(1);
        var btnAttr = new Dictionary<string, string[]>
        {
            { "refresh", new[] { "javascript:;", "btn btn-primary btn-refresh", "fa fa-refresh", "", "Refresh" } },
            { "add", new[] { "javascript:;", "btn btn-success btn-add", "fa fa-plus", "Add", "Add" } },
            { "edit", new[] { "javascript:;", "btn btn-success btn-edit btn-disabled disabled", "fa fa-pencil", "Edit", "Edit" } },
            { "del", new[] { "javascript:;", "btn btn-danger btn-del btn-disabled disabled", "fa fa-trash", "Delete", "Delete" } },
            { "import", new[] { "javascript:;", "btn btn-info btn-import", "fa fa-upload", "Import", "Import" } }
        };

        if (attr != null)
        {
            foreach (var entry in attr)
            {
                btnAttr[entry.Key] = entry.Value;
            }
        }

        var html = new List<string>();
        foreach (var btn in btns)
        {
            if (!btnAttr.TryGetValue(btn, out var value) || (btn != "refresh" && !await _currentUserService.CheckPermissionAsync($"{controller}/{btn}")))
            {
                continue;
            }
            var (href, className, icon, text, title) = (value[0], value[1], value[2], value[3], value[4]);
            if (btn == "import")
            {
                var template = controller?.Replace('.', '_');
                var download = "";

                if (File.Exists($"./template/{template}.xlsx"))
                {
                    download += $"<li><a href=\"/template/{template}.xlsx\" target=\"_blank\">XLSX模版</a></li>";
                }
                if (File.Exists($"./template/{template}.xls"))
                {
                    download += $"<li><a href=\"/template/{template}.xls\" target=\"_blank\">XLS模版</a></li>";
                }
                if (File.Exists($"./template/{template}.csv"))
                {
                    download += string.IsNullOrEmpty(download) ? "" : "<li class=\"divider\"></li>";
                    download += $"<li><a href=\"/template/{template}.csv\" target=\"_blank\">CSV模版</a></li>";
                }
                download += string.IsNullOrEmpty(download) ? "" : "\n                            ";

                if (!string.IsNullOrEmpty(download))
                {
                    html.Add($@"
                    <div class=""btn-group"">
                        <button type=""button"" href=""{href}"" class=""btn btn-info btn-import"" title=""{title}"" id=""btn-import-file"" data-url=""ajax/upload"" data-mimetype=""csv,xls,xlsx"" data-multiple=""false""><i class=""{icon}""></i> {text}</button>
                        <button type=""button"" class=""btn btn-info dropdown-toggle"" data-toggle=""dropdown"" title=""下载批量导入模版"">
                            <span class=""caret""></span>
                            <span class=""sr-only"">Toggle Dropdown</span>
                        </button>
                        <ul class=""dropdown-menu"" role=""menu"">{download}</ul>
                    </div>");
                }
                else
                {
                    html.Add($"<a href=\"{href}\" class=\"{className}\" title=\"{title}\" id=\"btn-import-file\" data-url=\"ajax/upload\" data-mimetype=\"csv,xls,xlsx\" data-multiple=\"false\"><i class=\"{icon}\"></i> {text}</a>");
                }
            }
            else
            {
                html.Add($"<a href=\"{href}\" class=\"{className}\" title=\"{title}\"><i class=\"{icon}\"></i> {text}</a>");
            }
        }

        return string.Join(" ", html);
    }
}
