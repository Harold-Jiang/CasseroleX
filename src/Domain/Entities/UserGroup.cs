namespace CasseroleX.Domain.Entities;
public class UserGroup :BaseAuditableEntity
{ 
    public string? Name { get; set; }
 
    public string? Rules { get; set; }
 
    public Status Status { get; set; }
}
