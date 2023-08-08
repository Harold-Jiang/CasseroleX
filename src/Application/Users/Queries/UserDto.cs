using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Users.Queries;
public class UserDto  : IMapFrom<User>
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

    public int GroupId { get; set; }
    public string? GroupsText { get; set; }
    public int LoginFailure { get; set; } 
    public Status Status { get; set; }
    public string? LoginIp { get; set; }
    public string? Mobile { get; set; }
    public byte Gender { get; set; }
    public DateTime? BirthDay { get; set; } 
    public string? Bio { get; set; }
    public decimal Money { get; set; } 
    public int Score { get; set; }
    public int Level { get; set; }  
    public string? JoinIp { get; set; } 
    public DateTime? JoinTime { get; set; }
    public int Successions { get; set; } 
    public int MaxSuccessions { get; set; } 
    public DateTime? PrevTime { get; set; }
}
