using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CasseroleX.Application.Attachments.Commands.CreateAttachmentCommand;
public class CreateAttachmentCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public string Category { get; set; } = "unclassed";
    public required IFormFile File { get; init; }
}
public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, Result>
{
    private readonly IUploadService _uploadService;

    public CreateAttachmentCommandHandler(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }

    public async Task<Result> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _uploadService.UploadFileAsync(request.File, request.UserId, request.Category, false, cancellationToken);
        if (result is not null)
        {
            return Result.Success(new { result.Url, fullurl =  _uploadService.GetFullUrl(result.Url ?? "") });
        }
        return Result.Failure();
    }
}
