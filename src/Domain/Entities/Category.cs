namespace CasseroleX.Domain.Entities;
public class Category : BaseAuditableEntity
{
   
    public int Pid { get; set; }
    
    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? NickName { get; set; }
  
    public string? Image { get; set; }
    
    public string? Keywords { get; set; }
    
    public string? Description { get; set; }

    public string? DiyName { get; set; }
 
    public int Weigh { get; set; }
  
    public Status Status { get; set; }
}
