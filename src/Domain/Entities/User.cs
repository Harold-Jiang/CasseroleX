namespace CasseroleX.Domain.Entities;
/// <summary>
/// 用户表
/// </summary>
public class User : BaseUser
{
    /// <summary>
    /// 性别 0未知 1男 2女
    /// </summary> 
    public byte Gender { get; set; }
  
    /// <summary>
    /// 真实姓名
    /// </summary> 
    public string? RealName { get; set; }
    /// <summary>
    /// 出生日期
    /// </summary> 
    public DateTime? BirthDay { get; set; }
    /// <summary>
    /// 身份证号
    /// </summary> 
    public string? IdCard { get; set; }
    /// <summary>
    /// 简介
    /// </summary> 
    public string? Bio { get; set; }
 
    /// <summary>
    /// 余额
    /// </summary> 
    public decimal Money { get; set; }
    /// <summary>
    /// 积分
    /// </summary> 
    public int Score { get; set; }
     
    /// <summary>
    /// 用户组
    /// </summary>
    public int GroupId { get; set; }
    /// <summary>
    /// 用户等级
    /// </summary>  
    public int Level { get; set; }
    
    /// <summary>
    /// 所在区域
    /// </summary> 
    public int RegionId { get; set; }

    /// <summary>
    /// 加入IP
    /// </summary>
    public string? JoinIp { get; set; }

    /// <summary>
    /// 加入时间
    /// </summary>
    public DateTime? JoinTime { get; set; }

    /// <summary>
    /// 连续登录天数
    /// </summary>
    public int Successions { get; set; }
    /// <summary>
    /// 最多连续登录天数
    /// </summary>
    public int MaxSuccessions { get; set; }
    /// <summary>
    /// 上次登录时间
    /// </summary> 
    public DateTime? PrevTime { get; set; }
    /// <summary>
    /// 加入IP
    /// </summary>
    public string? RegisterIp { get; set; }

    /// <summary>
    /// 验证
    /// </summary>
    public Verification Verification { get; set; } = new Verification(false,false);
    
}
