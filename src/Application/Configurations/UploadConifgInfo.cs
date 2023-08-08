using System.Text.Json.Serialization;

namespace CasseroleX.Application.Configurations;

public partial class UploadConfigInfo : IConfig
{ 
    public string? UploadUrl { get; set; }
    
    public string? SaveKey { get; set; }
 
    public string? MaxSize { get; set; }
 
    public string? MimeType { get; set; }
    
    public bool Multiple { get; set; } = false;
 
    public bool Chunking { get; set; } = false;
    
    public int ChunkSize { get; set; } = 2097152;
  
    public bool FullMode { get; set; } = false;
 
    public string ThumbStyle { get; set; } = "";
 
    [JsonIgnore]
    public int MaximumImageSize { get; set; } = 2000;
    
    [JsonIgnore]
    public int DefaultImageQuality { get; set; } = 70;
 
    [JsonIgnore]
    public Dictionary<string, string> AttachmentCategory { get; set; } = new Dictionary<string, string>
    {
        {"image","Imgae"},
        {"zip","Zip"}

    };
     
    [JsonIgnore]
    public Dictionary<string, string> MimeTypeList => new()
    {
        {"Image","image/*"},
        {"Audio","audio/*"},
        {"Video","video/*"},
        {"Text","text/*"},
        {"Application","application/*"},
        {"Zip","zip,rar,7z,tar" }, 
    };
}
