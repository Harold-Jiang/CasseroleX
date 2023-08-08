using CasseroleX.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CasseroleX.Application.Common.Interfaces;
public interface IUploadService
{
    void DeleteFile(string path);

    /// <summary>
    /// get image full url
    /// </summary>
    string GetFullUrl(string? url, string storage = "local");

    /// <summary>
    /// upload file
    /// </summary>
    Task<Attachment> UploadFileAsync(IFormFile formFile, int userId, string category, bool isSaveDefaultFileName = false, CancellationToken cancellationToken = default);
}
