using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.UserRules.Queries;
public class UserRuleDto:TreeDto<UserRuleDto>, IMapFrom<UserRule>
{
    public string Rules { get; set; } = null!;
    public string? Remark { get; set; }

    public bool IsMenu { get; set; }

    public int Weigh { get; set; }
    public Status Status { get; set; }

}
