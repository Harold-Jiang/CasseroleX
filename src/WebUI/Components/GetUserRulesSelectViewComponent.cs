using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.UserRules.Queries;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class GetUserRulesSelectViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context; 
    private readonly IMapper _mapper;

    public GetUserRulesSelectViewComponent(IApplicationDbContext context,  
        IMapper mapper)
    {
        _context = context; 
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(Dictionary<string,string>? dic = null,string selected = "")
    {
        var ruleList = await _context.UserRules
            .ProjectTo<UserRuleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var ruleData = new List<SelectListItem>();
        ruleData.Add(new SelectListItem { Value = "0", Text = "None" });
        var result = Tree.GetTreeList(Tree.GetTreeArray(ruleList, 0));
        foreach (var item in result)
        {
            ruleData.Add(new SelectListItem
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
        foreach (var rule in ruleData)
        {
            var optionTag = new TagBuilder("option");
            optionTag.Attributes.Add("value", rule.Value);
            optionTag.InnerHtml.Append(rule.Text.Replace("&nbsp;", " "));

            if (selectedIds.IsNotNullOrAny() && selectedIds.Contains(rule.Value))
            {
                optionTag.Attributes.Add("selected", "selected");
            }

            selectTag.InnerHtml.AppendHtml(optionTag);
        }
         
        return new HtmlContentViewComponentResult(selectTag);
    } 
}
