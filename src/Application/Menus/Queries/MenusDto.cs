
namespace CasseroleX.Application.Menus.Queries;
public class MenusDto
{
    public string MenuList { get; set; } = null!;
    public string NavList { get; set; } = null!;
    public MenuTreeDto? SelectedMenu { get; set; }
    public MenuTreeDto? RefererMenu { get; set; } 
}
