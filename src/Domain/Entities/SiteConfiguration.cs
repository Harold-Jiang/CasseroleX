namespace CasseroleX.Domain.Entities;


public class SiteConfiguration : BaseAuditableEntity
{ 
    
    public string Name { get; set; } = null!;
   
    public string Group { get; set; } = null!;
    
    public string? Title { get; set; }
    
    public string? Tip { get; set; }
   
    public string? Type { get; set; }
    
    public string? Visible { get; set; }
  
    public string? Value { get; set; }
    
    public string? Content { get; set; }
    
    public string? Rule { get; set; }
    
    public string? Extend { get; set; }
}
