using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities.Role;

namespace CasseroleX.Application.Menus.Queries;

public class MenuTreeDto : TreeDto<MenuTreeDto> ,IMapFrom<RolePermissions>
{
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? PY { get; set; }
    public string? PinYin { get; set; }
    public string Title { get; set; } = null!;
    public string? Badge { get; set; }
    public string? MenuClass { get; set; }
    public string? MenuTabs { get; set; } 
    public string? MenuType { get; set; }

    public string? Extend { get; set; }

} 
