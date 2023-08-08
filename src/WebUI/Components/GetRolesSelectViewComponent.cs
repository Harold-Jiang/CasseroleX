using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Json;
using CasseroleX.Application.Roles.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class GetRolesSelectViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleManager _roleManager;
    private readonly IMapper _mapper;

    public GetRolesSelectViewComponent(IApplicationDbContext context, 
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

        var _childrenAdminIds = await _roleManager.GetChildrenAdminIds(isSuperAdmin, User.Identity.ID(), isSuperAdmin);
        var _childrenRoleIds = await _roleManager.GetChildrenRoleIds(User.Identity.ID(), isSuperAdmin);

        var roleList = await _context.Roles
            .Where(g => _childrenRoleIds.Contains(g.Id))
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var roleData = new List<SelectListItem>();
        if (isSuperAdmin)
        {
            var result = Tree.GetTreeList(Tree.GetTreeArray(roleList,0));
            foreach (var item in result)
            {
                roleData.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name,
                });
            }
        }
        else
        { 
            var roles = await _roleManager.GetRolesAsync(User.Identity.ID());
            foreach (var role in roles)
            {
                var childList = Tree.GetTreeList(Tree.GetTreeArray(roleList,role.Id));
                var temp = new Dictionary<int, string>();
                foreach (var item in childList)
                {
                    temp[item.Id] = item.Name;
                }
                roleData.Add(new SelectListItem
                {
                    Value = temp.ToJson(),
                    Text = role.Name,
                });
            } 
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
        foreach (var role in roleData)
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
