using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Admins.Queries;
public class AdminDto  : IMapFrom<Admin>
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? NickName { get; set; }
    public string? Avatar { get; set; }
    public DateTime? LoginTime { get; set; }
    public string Email { get; set; } = null!;
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? RoleIds { get; set; }
    public string? GroupsText { get; set; }
    public int LoginFailure { get; set; } 
    public Status Status { get; set; }
    public string? LoginIp { get; set; }
    public string? Mobile { get; set; }
    
}
