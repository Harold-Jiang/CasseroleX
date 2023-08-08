using System.Security.Cryptography;
using System.Text;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Singleton;
using CasseroleX.Application.Configurations;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using CasseroleX.Domain.Enums;
using CasseroleX.Infrastructure.OptionSetup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace CasseroleX.Infrastructure.Services;
public class UploadService : IUploadService
{
    private readonly ISiteConfigurationService _siteConfigurationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;
    private readonly AppOptions _app;
    private readonly SystemConfigInfo _systemConfig;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UploadService(ISiteConfigurationService siteConfigurationService,
        IApplicationDbContext context,
        IOptions<AppOptions> app,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _siteConfigurationService = siteConfigurationService;
        _context = context;
        _app = app.Value;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _systemConfig =  _siteConfigurationService.GetConfigurationAsync<SystemConfigInfo>().GetAwaiter().GetResult();

    }

  

    /// <summary>
    /// upload file
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="userId"></param>
    /// <param name="category"></param>
    /// <param name="isSaveDefaultFileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="IOException"></exception>
    public async Task<Attachment> UploadFileAsync(IFormFile formFile, int userId, string category, bool isSaveDefaultFileName = false, CancellationToken cancellationToken = default)
    {
        var _upload = await _siteConfigurationService.GetConfigurationAsync<UploadConfigInfo>();
        var fileName = Path.GetFileName(formFile.FileName);

        //Obtain suffix based on file name
        var fileExt = Path.GetExtension(fileName).Trim('.');
        if (!string.IsNullOrEmpty(fileExt))
            fileExt = fileExt.ToLowerInvariant();

        #region file check
        //Check whether the Filename extension is legal
        if (!CheckExecutable(fileExt, _upload))
            throw new IOException($"Uploading files of type {fileExt} is not allowed");
        //Check if the file size is legal
        if (!CheckFileSize(formFile.Length, _upload))
            throw new IOException($"The file exceeds the limit and is limited in size to {_upload. MaxSize} bytes");
        #endregion

        #region MimeType
        var contentType = formFile.ContentType;

        if (string.IsNullOrEmpty(contentType) || contentType == MimeTypes.ImageSvg)
        {
            switch (fileExt)
            {
                case ".bmp":
                    contentType = MimeTypes.ImageBmp;
                    break;
                case ".gif":
                    contentType = MimeTypes.ImageGif;
                    break;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    contentType = MimeTypes.ImageJpeg;
                    break;
                case ".webp":
                    contentType = MimeTypes.ImageWebp;
                    break;
                case ".png":
                    contentType = MimeTypes.ImagePng;
                    break;
                case ".svg":
                    contentType = MimeTypes.ImageSvg;
                    break;
                case ".tiff":
                case ".tif":
                    contentType = MimeTypes.ImageTiff;
                    break;
                default:
                    break;
            }
        }
        #endregion

        //Upload file conversion btye array
        var filebtyes = await GetDownloadBitsAsync(formFile);

        //Check if the SHA1 value in the file is duplicated
        var sha1 = GetFileSha1(filebtyes);

        var attachment = await _context.Attachments.FirstOrDefaultAsync(x => x.Sha1 == sha1);
        if (attachment is not null)
        {
            return attachment; 
        }

        //File name processing
        var guidName = $"{RandomProvider.GetGuid32ToString()}.{fileExt}";
        if (!isSaveDefaultFileName) //不保存原始文件名为GUID
            fileName = guidName;

        //File upload path processing
        var dicPath = GetSaveFilePath(_upload.SaveKey);
        var dicerctory = Path.GetFullPath(dicPath, _webHostEnvironment.WebRootPath);
        if (!Directory.Exists(dicerctory))
        {
            Directory.CreateDirectory(dicerctory);
        }

        Attachment model = new()
        {
            UserId = userId,
            Category = category,
            FileName = fileName,
            FileSize = filebtyes.Length,
            ImageWidth = 0,
            ImageHeight = 0,
            ImageType = fileExt,
            ImageFrames = 0,
            MimeType = contentType,
            Url = $"/{dicPath}{guidName}",
            //model.CreateTime = DateTime.Now;
            UploadTime = DateTime.Now,
            Storage = "local",
            Sha1 = sha1,
            ExtParam = ""
        };

        //Check if the file has image attributes assigned to them in the file object
        if (CheckImage(fileExt))
        {
            filebtyes = await ValidateImageAsync(filebtyes, contentType, _upload, ref model);
        }
        var filePath = Path.Combine(new string[] { dicerctory, guidName });
        await File.WriteAllBytesAsync(filePath, filebtyes, cancellationToken);
        await _context.Attachments.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return model;
    }


    public void DeleteFile(string path)
    {
        var fullPath =  Path.GetFullPath(path.StartsWith("/") ? path[1..]:path, _webHostEnvironment.WebRootPath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath); 
        }
    }

    /// <summary>
    /// get image full url
    /// </summary>
    /// <param name="url"></param>
    /// <param name="storage"></param>
    /// <returns></returns>
    public string GetFullUrl(string? url, string storage = "local")
    {
        if (!url.IsNotNullOrEmpty())
        {
            return "";
        }
        if (storage != "local")
        {
            //Cloud storage
        }

        if (_systemConfig.CDN.IsNotNullOrEmpty()) //cdn
        {
            return $"{_systemConfig.CDN}{url}";
        }

        return $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{url}";
    }

    /// <summary>
    /// Obtain file save path
    /// </summary> 
    private static string GetSaveFilePath(string? uploadSaveKey)
    {
        var saveKey = string.Empty;
        if (!string.IsNullOrEmpty(uploadSaveKey))
        {
            saveKey = uploadSaveKey.Replace("{filemd5}", "").Replace("{.suffix}", "");
            saveKey = saveKey.Replace("{year}", DateTime.Now.ToString("yyyy"))
                .Replace("{mon}", DateTime.Now.ToString("MM"))
                .Replace("{day}", DateTime.Now.ToString("dd"))
                .Replace("{hour}", DateTime.Now.ToString("HH"))
                .Replace("{min}", DateTime.Now.ToString("mm"))
                .Replace("{sec}", DateTime.Now.ToString("ss"))
                .Replace("{random}", RandomProvider.Number(16))
                .Replace("{random32}", RandomProvider.Number(32));
        }
        else
        {
            saveKey = $"{DateTime.Now:yyyyMMddHHmmss}{RandomProvider.Number(16)}";
        }
        
        if (saveKey.StartsWith("/"))
        {
            return saveKey[1..];
        }
        return saveKey;
    }


    /// <summary>
    /// Obtain downloaded binary array
    /// </summary>
    private static async Task<byte[]> GetDownloadBitsAsync(IFormFile file)
    {
        await using var fileStream = file.OpenReadStream();
        await using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        var fileBytes = ms.ToArray();

        return fileBytes;
    }

    /// <summary>
    /// Check file size
    /// </summary>
    private static bool CheckFileSize(long fileSize, UploadConfigInfo _upload)
    {
        if (_upload.MaxSize is null)
            _upload.MaxSize = "10m"; //如果没有设置上传文件最大字节 默认10m
        _upload.MaxSize.preg_match(@"([0-9\.]+)(\w+)", out List<string> matches);

        int size = matches.Any() ? int.Parse(matches[1]) : 0;
        var type = matches.Any() ? matches[2].ToLower() : "b";

        Dictionary<string, int> typeDict =
            new()
            {
                    { "b", 0 },
                    { "k", 1 },
                    { "kb", 1 },
                    { "m", 2 },
                    { "mb", 2 },
                    { "gb", 3 },
                    { "g", 3 },
            };

        typeDict.TryGetValue(type, out int typeValue);
        if (fileSize > (long)(size * Math.Pow(1024, typeValue)))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Check if it is an image file
    /// </summary>
    /// <param name="fileExt">file extension，does not contain“.”</param>
    private static bool CheckImage(string fileExt)
    {
        var imgExt =
            new List<string>
            {
                    "bmp",
                    "gif",
                    "webp",
                    "jpeg",
                    "jpg",
                    "jpe",
                    "jfif",
                    "pjpeg",
                    "pjp",
                    "png",
                    "tiff",
                    "tif",
                    "svg"
            } as IReadOnlyCollection<string>;

        if (imgExt.Any(ext => ext.Equals(fileExt, StringComparison.CurrentCultureIgnoreCase)))
            return true;
        return false;
    }

    /// <summary>
    /// Verify the size of the image
    /// </summary> 
    private static Task<byte[]> ValidateImageAsync(
        byte[] pictureBinary,
        string mimeType,
        UploadConfigInfo _upload,
        ref Attachment model
    )
    {
        try
        {
            using var image = SKBitmap.Decode(pictureBinary);
            model.ImageHeight = image.Height;
            model.ImageWidth = image.Width;
            // Resize the image according to its maximum size
            if (Math.Max(image.Height, image.Width) > _upload.MaximumImageSize)
            {
                var format = GetImageFormatByMimeType(mimeType);
                pictureBinary = ImageResize(image, format, _upload);
            }
            return Task.FromResult(pictureBinary);
        }
        catch
        {
            return Task.FromResult(pictureBinary);
        }
    }

    /// <summary>
    /// Obtain image format by mime type
    /// </summary>
    private static SKEncodedImageFormat GetImageFormatByMimeType(string mimeType)
    {
        var format = SKEncodedImageFormat.Jpeg;
        if (string.IsNullOrEmpty(mimeType))
            return format;

        var parts = mimeType.ToLowerInvariant().Split('/');
        var lastPart = parts[^1];

        switch (lastPart)
        {
            case "webp":
                format = SKEncodedImageFormat.Webp;
                break;
            case "png":
            case "gif":
            case "bmp":
            case "x-icon":
                format = SKEncodedImageFormat.Png;
                break;
            default:
                break;
        }

        return format;
    }

    /// <summary>
    /// Resize image by targetSize
    /// </summary>
    private static byte[] ImageResize(
        SKBitmap image,
        SKEncodedImageFormat format,
        UploadConfigInfo _upload
    )
    {
        if (image == null)
            throw new NotFoundException(nameof(image));

        float width,
            height;
        if (image.Height > image.Width)
        {
            // portrait
            width = image.Width * (_upload.MaximumImageSize / (float)image.Height);
            height = _upload.MaximumImageSize;
        }
        else
        {
            // landscape or square
            width = _upload.MaximumImageSize;
            height = image.Height * (_upload.MaximumImageSize / (float)image.Width);
        }

        if ((int)width == 0 || (int)height == 0)
        {
            width = image.Width;
            height = image.Height;
        }
        try
        {
            using var resizedBitmap = image.Resize(
                new SKImageInfo((int)width, (int)height),
                SKFilterQuality.Medium
            );
            using var cropImage = SKImage.FromBitmap(resizedBitmap);

            //In order to exclude saving pictures in low quality at the time of installation, we will set the value of this parameter to 80 (as by default)
            return cropImage
                .Encode(
                    format,
                    _upload.DefaultImageQuality > 0 ? _upload.DefaultImageQuality : 80
                )
                .ToArray();
        }
        catch
        {
            return image.Bytes;
        }
    }

    /// <summary>
    /// Check hazardous documents
    /// </summary>
    /// <returns></returns>
    private static bool CheckExecutable(string fileExt, UploadConfigInfo _upload)
    {
        string[] excExt =
        {
                "asp",
                "aspx",
                "ashx",
                "asa",
                "asmx",
                "asax",
                "php",
                "jsp",
                "htm",
                "html"
            };
        if (excExt.Any(ext => ext.Equals(fileExt, StringComparison.CurrentCultureIgnoreCase)))
            return false;

        //Check legal documents
        var allowExt = _upload.MimeType.ToIList<string>();
        if (allowExt != null && allowExt.Any(ext => ext.Equals(fileExt, StringComparison.CurrentCultureIgnoreCase)))
            return true;

        return false;
    }

    /// <summary>
    /// Obtain the SHA1 value of the file
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private static string GetFileSha1(byte[] bytes)
    {
        if (Singleton<SHA1>.Instance == null)
        {
            Singleton<SHA1>.Instance = SHA1.Create();
        }
        SHA1 sha1 = Singleton<SHA1>.Instance;
        var buffer = sha1.ComputeHash(bytes);
        //Convert byte array to hexadecimal string form
        StringBuilder stringBuilder = new();
        for (int i = 0; i < buffer.Length; i++)
        {
            stringBuilder.Append(buffer[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }

}
