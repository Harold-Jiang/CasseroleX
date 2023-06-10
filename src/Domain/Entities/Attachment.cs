namespace CasseroleX.Domain.Entities;

public class Attachment : BaseAuditableEntity
{
   
    /// <summary>
    /// 文件的sha1编码
    /// </summary>
    public string? Sha1 { get; set; }
    /// <summary>
    /// 储存位置
    /// </summary>
  
    public string? Storage { get; set; } 
    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime? UploadTime { get; set; }

    /// <summary>
    /// 透传数据
    /// </summary>
    public string? ExtParam { get; set; }
    /// <summary>
    /// MimeType类型
    /// </summary>
    public string? MimeType { get; set; }
    /// <summary>
    /// 文件大小
    /// </summary>
    public int FileSize { get; set; }
    /// <summary>
    /// 文件名称
    /// </summary>
    public string? FileName { get; set; }
    /// <summary>
    /// 图片帧数
    /// </summary>
    public int ImageFrames { get; set; } 
    /// <summary>
    /// 图片类型
    /// </summary>
    public string? ImageType { get; set; }
    /// <summary>
    /// 图片高度
    /// </summary>
    public int ImageHeight { get; set; }
    /// <summary>
    /// 图片宽度
    /// </summary>
    public int ImageWidth { get; set; }
    /// <summary>
    /// 物理路径
    /// </summary>
    public string? Url { get; set; }
    /// <summary>
    /// 会员ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 类别
    /// </summary>
    public string? Category { get; set; }
     
 
}
