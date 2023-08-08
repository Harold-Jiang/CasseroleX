
namespace CasseroleX.Application.Menus.Queries;
public class HomeMenuDto
{
    public string MenuList { get; set; } = null!;
    public string NavList { get; set; } = null!;
    public MenuDto? SelectedMenu { get; set; }
    public MenuDto? RefererMenu { get; set; } 
}
