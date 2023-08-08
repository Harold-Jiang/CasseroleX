using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.UserGroups.Queries;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class GetUserGroupsSelectViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper;

    public GetUserGroupsSelectViewComponent(IApplicationDbContext context,  
        IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(Dictionary<string,string>? dic = null,string selected = "")
    {
        var groupList = await _context.UserGroups
            .ProjectTo<UserGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var groupData = new List<SelectListItem>();
        
        foreach (var item in groupList)
        {
            groupData.Add(new SelectListItem
            {
                Value = item.Id.ToString(),
                Text = item.Name,
            });
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
        foreach (var group in groupData)
        {
            var optionTag = new TagBuilder("option");
            optionTag.Attributes.Add("value", group.Value);
            optionTag.InnerHtml.Append(group.Text.Replace("&nbsp;", " "));

            if (selectedIds.IsNotNullOrAny() && selectedIds.Contains(group.Value))
            {
                optionTag.Attributes.Add("selected", "selected");
            }

            selectTag.InnerHtml.AppendHtml(optionTag);
        }
         
        return new HtmlContentViewComponentResult(selectTag);
    } 
}
