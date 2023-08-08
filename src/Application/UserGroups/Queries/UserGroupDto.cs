using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.UserGroups.Queries;
public class UserGroupDto :IMapFrom<UserGroup>
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public string? Rules { get; set; }

    public Status Status { get; set; }
}
