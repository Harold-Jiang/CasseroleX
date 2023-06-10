using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Menus.Queries;

public record GetMenusQuery : IRequest<MenusDto>
{
    public List<string> PermissionIds { get; set; } = null!;

    //public int AdminId { get; set; }
    public string? RefererUrl { get; set; }
    public bool ShowSubMenu { get; set; }
    public bool MultipleNav { get; set; }
};

public class GetMenuTreeHandler : IRequestHandler<GetMenusQuery, MenusDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper; 
    public GetMenuTreeHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MenusDto> Handle(GetMenusQuery request, CancellationToken cancellationToken)
    {
        // 筛选条件
        Expression<Func<RolePermissions, bool>> where = x => request.PermissionIds.Contains(x.Id.ToString()) 
           && x.IsMenu == 1
           && x.Status == Status.normal;
        if (request.PermissionIds.Contains("*")) //supadmin
        {
            where = x => x.IsMenu == 1 && x.Status == Status.normal;
        }
        var rules = await _context.RolePermissions
                        .AsNoTracking()
                        .Where(where)
                        .ProjectTo<MenuTreeDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

        //获取菜单列表 
        return GetSidebarAsync(rules, new Dictionary<string, object>
        {
            {"dashboard","hot"},
            {"addon" ,new string []{"new", "red", "badge"}},
            {"auth/group" ,"Menu" },
            {"general" ,new string []{"new", "purple"}},

        }, request.RefererUrl, request.ShowSubMenu, request.MultipleNav, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Get left and top menu bars
    /// </summary>
    private static MenusDto GetSidebarAsync(List<MenuTreeDto> ruleList, Dictionary<string, object> parameters, string? refererUrl, bool showSubMenu, bool multipleNav, string fixedPage = "dashboard", CancellationToken cancellationToken = default)
    {
        MenusDto sidebar = new();
        //Hook.Listen("admin_sidebar_begin", parameters);
        string[] colorArr = { "red", "green", "yellow", "blue", "teal", "orange", "purple" };
        int colorNums = colorArr.Length;
        Dictionary<string, string> badgeList = new();
        foreach (var param in parameters)
        {
            string url = param.Key;
            if (param.Value is string)
            {
                string nums = (string)param.Value;
                string color = colorArr[(int)(char.IsDigit(nums[0]) ? nums[0] - '0' : nums.Length) % colorNums];
                string className = "label";

                if (!string.IsNullOrEmpty(nums))
                {
                    badgeList[url] = "<small class=\"" + className + " pull-right bg-" + color + "\">" + nums + "</small>";
                }
            }
            else if (param.Value is object[] arr)
            {
                string nums = arr.Length > 0 ? arr[0]?.ToString() ?? "" : "0";
                string color = arr.Length > 1 ? arr[1]?.ToString() ?? "" : colorArr[(int)(char.IsDigit(nums[0]) ? nums[0] - '0' : nums.Length) % colorNums];
                string className = arr.Length > 2 ? arr[2]?.ToString() ?? "" : "label";

                if (!string.IsNullOrEmpty(nums))
                {
                    badgeList[url] = "<small class=\"" + className + " pull-right bg-" + color + "\">" + nums + "</small>";
                }
            }
        }

        //HashSet<string> userRule = await _roleManager.GetPermissionsAsync(AdminId, cancellationToken);

        //var ruleList = await _context.RolePermissions
        //                .AsNoTracking()
        //                .Where(x => x.Status == Status.normal && x.IsMenu == 1)
        //                .OrderByDescending(x => x.Weigh)
        //                .ProjectTo<MenuTreeDto>(_mapper.ConfigurationProvider)
        //                .ToListAsync(cancellationToken);

        //var indexRuleList = await _context.RolePermissions
        //                .AsNoTracking()
        //                .Where(x => x.Status == Status.normal && x.IsMenu == 0 && x.Name.EndsWith("/index"))
        //                .Select(x => x.Name)
        //                .ToListAsync(cancellationToken);

        //var pidArr = ruleList.Select(x => x.Pid).Distinct().ToArray();

        for (int i = 0; i < ruleList.Count; i++)
        {
            var item = ruleList[i];
            //if (!userRule.Contains(item.Name))
            //{
            //    ruleList.Remove(item);
            //    continue;
            //}
            //string indexRuleName = $"{item.Name}/index";
            //if (indexRuleList.Contains(indexRuleName) && !userRule.Contains(indexRuleName))
            //{
            //    ruleList.Remove(item);
            //    continue;
            //}
            item.Icon = $"{item.Icon} fa-fw";
            item.Url = !string.IsNullOrEmpty(item.Url) ? item.Url : $"/{item.Name}";
            item.Badge = badgeList.ContainsKey(item.Name) ? badgeList[item.Name] : "";
            item.Title = item.Title;
            item.Url = item.Url.preg_match(@"^((?:[a-z]+:)?\/\/|data:image\/)(.*)") ? item.Url : item.Url;
            item.MenuClass = new[] { "dialog", "ajax" }.Contains(item.MenuType) ? $"btn-{item.MenuType}" : "";
            item.MenuTabs = string.IsNullOrEmpty(item.MenuType) || (new[] { "default", "addtabs" }).Contains(item.MenuType) ? $"addtabs=\"{item.Id}\"" : "";
            sidebar.SelectedMenu = item.Name == fixedPage ? item : sidebar.SelectedMenu;
            sidebar.RefererMenu = item.Url == refererUrl ? item : sidebar.RefererMenu;
        }
        //var lastArr = ruleList.Select(x => x.Pid).Distinct().ToArray();
        //var pidDiffArr = pidArr.Except(lastArr).ToArray();
        //for (int i = 0; i < ruleList.Count; i++)
        //{
        //    if (pidDiffArr.Contains(ruleList[i].Id))
        //    {
        //        ruleList.Remove(ruleList[i]);
        //    }
        //}

        if (sidebar.SelectedMenu != null && sidebar.RefererMenu != null && sidebar.SelectedMenu.Id == sidebar.RefererMenu.Id)
        {
            sidebar.RefererMenu = null;
        }

        var select_id = sidebar.RefererMenu != null ? sidebar.RefererMenu.Id : (sidebar.SelectedMenu != null ? sidebar.SelectedMenu.Id : 0);

        var showSubmenu = showSubMenu;
        if (multipleNav)
        {
            var topList = ruleList.Where(x => x.Pid > 0).ToList();
            var selectParentIds = new List<int>();

            if (select_id > 0)
            {
                selectParentIds = Tree.GetParentsIds(ruleList, select_id, true);
            }
            foreach (var item in topList)
            {
                var childList = GetTreeMenu(
                    topList,
                    item.Id,
                    "<li class=\"@class\" pid=\"@pid\"><a @extend href=\"@url@addtabs\" addtabs=\"@id\" class=\"@menuclass\" url=\"@url\" py=\"@py\" pinyin=\"@pinyin\"><i class=\"@icon\"></i> <span>@title</span> <span class=\"pull-right-container\">@caret @badge</span></a> @childlist</li>",
                    new List<int> { select_id },
                    null,
                    "ul",
                    $"class=\"treeview-menu{(showSubmenu ? " menu-open" : "")}\""
                );

                bool current = selectParentIds.Contains(item.Id);
                var url = !string.IsNullOrEmpty(childList) ? "javascript:;" : item.Url;
                var addtabs = (!string.IsNullOrEmpty(childList) || string.IsNullOrEmpty(url)) ? "" : (url.Contains('?') ? "&" : "?") + "ref=" + (!string.IsNullOrEmpty(item.MenuType) ? item.MenuType : "addtabs");
                childList = childList.Replace($"\" pid=\"{item.Id}\"", $" {(current ? "" : "hidden")} pid=\"{item.Id}\"");

                sidebar.NavList += "<li class=\"" + (current ? "active" : "") + "\"><a " + (item.Extend ?? "") + " href=\"" + url + addtabs + "\" " + (item.MenuTabs ?? "") + " class=\"" + (item.MenuClass ?? "") + "\" url=\"" + url + "\" title=\"" + item.Title + "\"><i class=\"" + item.Icon + "\"></i> <span>" + item.Title + "</span> <span class=\"pull-right-container\"> </span></a> </li>";
                sidebar.MenuList += childList;

            }

        }
        else
        {
            // Building Menu Data
            sidebar.MenuList = GetTreeMenu(
                ruleList,
                0,
                "<li class=\"@class\"><a @extend href=\"@url@addtabs\" @menutabs class=\"@menuclass\" url=\"@url\" py=\"@py\" pinyin=\"@pinyin\"><i class=\"@icon\"></i> <span>@title</span> <span class=\"pull-right-container\">@caret @badge</span></a> @childlist</li>",
                new List<int> { select_id },
                null,
                "ul",
                "class=\"treeview-menu" + (showSubmenu ? " menu-open" : "") + "\""
            );
            if (sidebar.SelectedMenu != null)
            {
                sidebar.NavList += "<li role=\"presentation\" id=\"tab_" + sidebar.SelectedMenu.Id + "\" class=\"" + (sidebar.RefererMenu == null ? "" : "active") + "\"><a href=\"#con_" + sidebar.SelectedMenu.Id + "\" node-id=\"" + sidebar.SelectedMenu.Id + "\" aria-controls=\"" + sidebar.SelectedMenu.Id + "\" role=\"tab\" data-toggle=\"tab\"><i class=\"" + sidebar.SelectedMenu.Icon + " fa-fw\"></i> <span>" + sidebar.SelectedMenu.Title + "</span> </a></li>";
            }
            if (sidebar.RefererMenu != null)
            {
                sidebar.NavList += "<li role=\"presentation\" id=\"tab_" + sidebar.RefererMenu.Id + "\" class=\"active\"><a href=\"#con_" + sidebar.RefererMenu.Id + "\" node-id=\"" + sidebar.RefererMenu.Id + "\" aria-controls=\"" + sidebar.RefererMenu.Id + "\" role=\"tab\" data-toggle=\"tab\"><i class=\"" + sidebar.RefererMenu.Icon + " fa-fw\"></i> <span>" + sidebar.RefererMenu.Title + "</span> </a> <i class=\"close-tab fa fa-remove\"></i></li>";
            }

        }

        return sidebar;
    }


    /// <summary>
    /// Building Menu Data
    /// </summary>
    public static string GetTreeMenu(List<MenuTreeDto> list, int myid, string itemtpl, List<int>? selectedids = default, List<int>? disabledids = default, string wraptag = "ul", string wrapattr = "", int deeplevel = 0, bool show_submenu = false)
    {
        StringBuilder str = new();
        List<MenuTreeDto> childs = Tree.GetChild(list, myid);

        if (childs.Count > 0)
        {
            foreach (var value in childs)
            {
                string tempItemTpl = itemtpl;  

                string selected = (selectedids != null && selectedids.Contains(value.Id)) ? "selected" : "";
                string disabled = (disabledids != null && disabledids.Contains(value.Id)) ? "disabled" : "";
                //object to dic
                var valueDict = value.GetType().GetProperties() 
                    .ToDictionary(
                        prop => "@" + prop.Name.ToLowerInvariant(),
                        prop => prop.GetValue(value)?.ToString()
                    )
                    ;
                valueDict["@selected"] = selected;
                valueDict["@disabled"] = disabled;

                Dictionary<string, string?> bakvalueDict = new();
                // Extract specific key value pairs to the bakvalue dictionary
                string[] keysToExtract = { "@url", "@caret", "@class" ,"@childlist" };
                foreach (string key in keysToExtract)
                {
                    if (valueDict.ContainsKey(key))
                    {
                        bakvalueDict[key] = valueDict[key];
                        valueDict.Remove(key);
                    }
                }
                //Replace 
                foreach (var entry in valueDict)
                {
                    tempItemTpl = tempItemTpl.Replace(entry.Key, entry.Value);
                }

                // Merge bakvalue dictionary back into value dictionary
                foreach (var kvp in bakvalueDict)
                {
                    valueDict.Add(kvp.Key, kvp.Value);
                }
                string childdata = GetTreeMenu(list, value.Id, itemtpl, selectedids, disabledids, wraptag, wrapattr, deeplevel + 1, show_submenu);
                string childlist = !string.IsNullOrEmpty(childdata) ? $"<{wraptag} {wrapattr}>{childdata}</{wraptag}>" : "";

                childlist = childlist.Replace("@class", !string.IsNullOrEmpty(childdata) ? "last" : "");

                var replacements = new Dictionary<string, string>()
                {
                    { "@childlist", childlist },
                    { "@url" ,!string.IsNullOrEmpty(childdata) || string.IsNullOrEmpty(value.Url) ? "javascript:;" : value.Url },
                    { "@addtabs", !string.IsNullOrEmpty(childdata) || string.IsNullOrEmpty(value.Url) ? "" : (value.Url.Contains('?') ? "&" : "?") + "ref=addtabs"},
                    {"@caret", (!string.IsNullOrEmpty(childdata) && (!valueDict.ContainsKey("@badge") || string.IsNullOrEmpty(valueDict["@badge"]))) ? "<i class=\"fa fa-angle-left\"></i>" : ""},
                    { "@badge",valueDict.ContainsKey("@badge") ? valueDict["@badge"]??"" : ""},
                    { "@class",(!string.IsNullOrEmpty(selected) ? " active" : "") + (!string.IsNullOrEmpty(disabled) ? " disabled" : "") + (!string.IsNullOrEmpty(childdata) ? " treeview" + (show_submenu ? " treeview-open" : "") : "")},

                 };


                foreach (var entry in replacements)
                {
                    tempItemTpl = tempItemTpl.Replace(entry.Key, entry.Value);
                }

                str.Append(tempItemTpl);
            }
        }

        return str.ToString();
    }

}
