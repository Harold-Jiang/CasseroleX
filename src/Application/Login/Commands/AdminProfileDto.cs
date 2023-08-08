using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;

namespace CasseroleX.Application.Login.Commands;
public class AdminProfileDto: IMapFrom<Admin>
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? NickName { get; set; }
    public string? Avatar { get; set; } 
    public DateTime? LoginTime { get; set; } 
    public string Email { get; set; } = null!;
}
