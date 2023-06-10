using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Categories.Queries;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Components;


public class BuildCategorySelectViewComponentViewComponent : ViewComponent
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public BuildCategorySelectViewComponentViewComponent(IApplicationDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string name, string type, string? selected = null, IDictionary<string, object>? attr = null, IDictionary<string, string>? header = null)
    {
        var html = await GenerateCategorySelect(name,type,selected,attr,header);
        return new HtmlContentViewComponentResult(new HtmlString(html));
    }
    public async Task<string> GenerateCategorySelect(string name, string type, string? selected = null, IDictionary<string, object>? attr = null, IDictionary<string, string>? header = null)
    {
        var list = await _context.Categories
            .Where(x=>x.Type == type)
            .ProjectTo<CategoriesDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        var categoryList = Tree.GetTreeList(Tree.GetTreeArray(list,0));
        var categoryData = header ?? new Dictionary<string, string>();

        foreach (var category in categoryList)
        {
            categoryData[category.Id.ToString()] = category.Name;
        }

        attr ??= new Dictionary<string, object>();
        attr["id"] = $"c-{name}";
        attr["class"] = "form-control selectpicker";

        return BuildSelect(name, categoryData, selected??"", attr);
    }
    private static string BuildSelect(string name, IDictionary<string, string> options, string selected, IDictionary<string, object> attributes)
    {
        var select = new TagBuilder("select");
        select.Attributes["name"] = name;

        foreach (var option in options)
        {
            var optionTag = BuildOption(option.Key, option.Value, selected??"");
            select.InnerHtml.AppendHtml(optionTag);
        }

        foreach (var attribute in attributes)
        {
            select.Attributes[attribute.Key] = attribute.Value.ToString();
        }

        return select?.ToString() ?? "";
    }

    private static TagBuilder BuildOption(string value, string text, string selectedValue)
    {
        var option = new TagBuilder("option")
        {
            TagRenderMode = TagRenderMode.Normal
        };

        if (value == selectedValue)
        {
            option.Attributes["selected"] = "selected";
        }  
        option.InnerHtml.Append(text);

        return option; 
    }

}
