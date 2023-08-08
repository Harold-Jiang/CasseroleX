using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Menus.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class GetMenusSelectViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;

    public GetMenusSelectViewComponent(IApplicationDbContext context, 
        IRoleManager roleManager, 
        IMapper mapper)
    {
        _context = context;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(Dictionary<string,string>? dic = null,string selected = "")
    {

        bool isSuperAdmin = User.Identity.IsSuperAdmin(); 
        if (!isSuperAdmin)
        {
            throw new Exception("Access is allowed only to the super management group");
        }

        var menuList = await _context.RolePermissions
                         .OrderBy(x => x.Weigh)
                         .OrderBy(x => x.Id)
                         .ProjectTo<MenuDto>(_mapper.ConfigurationProvider)
                         .ToListAsync();
       
  
        var menuTreeList = Tree.GetTreeList(Tree.GetTreeArray(menuList, 0), "Title");
        var menuData = new List<SelectListItem>();
        menuData.Add(new SelectListItem { Value = "0" ,Text = "None"});

        foreach (var menu in menuTreeList)
        {
            if (!menu.IsMenu)
            {
                continue;
            }
            menuData.Add(new SelectListItem { Value = menu.Id.ToString(), Text = menu.Title }); 
            menu.Spacer = null;
        }
 
        var selectTag = new TagBuilder("select"); 
        if (dic is not null)
        {
            foreach (var item in dic)
            {
                selectTag.Attributes.Add(item.Key, item.Value);
            }
        }

        selectTag.Attributes.Add("class", "form-control selectpicker");
        selectTag.Attributes.Add("data-rule", "required");

        var selectedIds = selected.ToIList<string>();
        foreach (var role in menuData)
        {
            var optionTag = new TagBuilder("option");
            optionTag.Attributes.Add("value", role.Value);
            optionTag.InnerHtml.Append(role.Text.Replace("&nbsp;", " "));

            if (selectedIds.IsNotNullOrAny() && selectedIds.Contains(role.Value))
            {
                optionTag.Attributes.Add("selected", "selected");
            }

            selectTag.InnerHtml.AppendHtml(optionTag);
        }
         
        return new HtmlContentViewComponentResult(selectTag);
    } 
}
