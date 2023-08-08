namespace CasseroleX.Domain.Entities;
public class UserRule : BaseAuditableEntity
{ 
    public int Pid { get; set; }
 
    public string  Name { get; set; } = null!;

    public string? Title { get; set; }
     
    public string? Remark { get; set; }

    public bool IsMenu { get; set; }
     
    public int Weigh { get; set; }

    public Status Status { get; set; }
}
