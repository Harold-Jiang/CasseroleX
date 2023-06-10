using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;

namespace CasseroleX.Application.Roles.Queries;
public class RolesDto : TreeDto<RolesDto>, IMapFrom<Role>
{  
    public string Rules { get; set; } = null!;
     
    public Status Status { get; set; }

}
