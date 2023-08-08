namespace CasseroleX.Domain.Entities;

public class Attachment : BaseAuditableEntity
{ 
   
    public string? Sha1 { get; set; }
    
  
    public string? Storage { get; set; } 
  
    public DateTime? UploadTime { get; set; }

    
    public string? ExtParam { get; set; }
    
    public string? MimeType { get; set; }
    
    public int FileSize { get; set; }
    
    public string? FileName { get; set; }
    
    public int ImageFrames { get; set; } 
    
    public string? ImageType { get; set; }
     
    public int ImageHeight { get; set; }
     
    public int ImageWidth { get; set; }
    
    public string? Url { get; set; }
    
    public int UserId { get; set; }
 
    public string? Category { get; set; }
     
 
}
