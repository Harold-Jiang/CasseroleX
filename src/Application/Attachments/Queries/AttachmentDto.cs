using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;

namespace CasseroleX.Application.Attachments.Queries;
public class AttachmentDto : IMapFrom<Attachment>
{
    public int Id { get; set; }
    public DateTime CreateTime { get; set; } 
    public DateTime? UpdateTime { get; set; }

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
    public string? FullUrl { get; set; }
    public string ThumbStyle { get; set; } = "";
}
