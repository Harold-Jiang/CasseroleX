using System.Text.Json.Serialization;

namespace CasseroleX.Application.Configurations;

public partial class UploadConfigInfo : IConfig
{

    /// <summary>
    ///  上传地址,默认是本地上传
    /// </summary>
    public string? UploadUrl { get; set; }
    /// <summary>
    /// 文件保存格式
    /// </summary>
    public string? SaveKey { get; set; }
    /// <summary>
    /// 文件上传大小
    /// </summary>
    public string? MaxSize { get; set; }
    /// <summary>
    /// 可上传的文件类型
    /// </summary>
    public string? MimeType { get; set; }
    /// <summary>
    /// 是否支持批量上传
    /// </summary>
    public bool Multiple { get; set; } = false;
    /// <summary>
    /// 是否支持分片上传
    /// </summary>
    public bool Chunking { get; set; } = false;
    /// <summary>
    /// 默认分片大小
    /// </summary>
    public int ChunkSize { get; set; } = 2097152;
    /// <summary>
    /// 完整URL模式
    /// </summary>
    public bool FullMode { get; set; } = false;
    /// <summary>
    /// 缩略图样式
    /// </summary>
    public string ThumbStyle { get; set; } = "";
    /// <summary>
    /// 图片最大尺寸
    /// </summary>
    [JsonIgnore]
    public int MaximumImageSize { get; set; } = 2000;
    /// <summary>
    /// 缩放图片默认质量
    /// </summary>
    [JsonIgnore]
    public int DefaultImageQuality { get; set; } = 70;

    /// <summary>
    /// 上传配置的分类
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string> AttachmentCategory { get; set; } = new Dictionary<string, string>
    {
        {"image","图片"},
        {"zip","压缩包"}

    };
    /// <summary>
    /// 上传文件类型列表
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, string> MimeTypeList => new()
    {
        {"图片","image/"},
        {"音频","audio/"},
        {"视频","video/"},
        {"文档","text/"},
        {"应用","application/"},
        //{"压缩包","zip,rar,7z,tar" },
    };
}
