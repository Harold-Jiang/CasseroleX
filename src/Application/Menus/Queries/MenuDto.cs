using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Menus.Queries;

public class MenuDto : TreeDto<MenuDto> ,IMapFrom<RolePermissions>
{
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? PY { get; set; }
    public string? PinYin { get; set; }
    public string? Badge { get; set; }
    public string? MenuClass { get; set; }
    public string? MenuTabs { get; set; } 
    public string? MenuType { get; set; }
    public bool IsMenu { get; set; }
    public string? Extend { get; set; }
    public int Weigh { get; set; }
    public string? Condition { get; set; }

    public string? Remark { get; set; }
    public Status Status { get; set; }

} 
