namespace CasseroleX.Domain.Entities.Role;

public partial class AdminRole
{ 
    public int AdminId { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual Admin Admin { get; set; } = null!;
}
