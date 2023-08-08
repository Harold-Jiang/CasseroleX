namespace CasseroleX.Domain.Entities;

public class AdminLog 
{

    public int Id { get; set; }
    public int AdminId { get; set; }

    public string UserName { get; set; } = null!;
 
    public string? Url { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }
  
    public string? Ip { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreateTime { get; set; } 

}