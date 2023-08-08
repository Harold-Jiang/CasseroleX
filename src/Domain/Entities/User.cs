namespace CasseroleX.Domain.Entities;

public class User : BaseUser
{
    
    public byte Gender { get; set; }
    public DateTime? BirthDay { get; set; }
  
    public string? RealName { get; set; }
  
    public string? IdCard { get; set; }
   
    public string? Bio { get; set; }
 
    public decimal Money { get; set; }
    
    public int Score { get; set; }
     
    public int GroupId { get; set; }
    
    public int Level { get; set; }
    
    public int RegionId { get; set; }

    public string? JoinIp { get; set; }

    public DateTime? JoinTime { get; set; }

    public int Successions { get; set; }
    
    public int MaxSuccessions { get; set; }
   
    public DateTime? PrevTime { get; set; }
    
    public string? RegisterIp { get; set; }

    Verification? _Verification  ;
    public Verification Verification
    {
        get => _Verification ??= new Verification(false, false);
        set => _Verification = value;
    }
    public virtual UserGroup? Group { get; set; }
}
