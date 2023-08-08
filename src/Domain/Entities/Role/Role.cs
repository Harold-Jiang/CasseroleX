
namespace CasseroleX.Domain.Entities.Role;

public partial class Role : BaseAuditableEntity
{
  
    public int Pid { get; set; }

    public string? Name { get; set; }

    public string Rules { get; set; } = null!;

    public Status Status { get; set; }
}
