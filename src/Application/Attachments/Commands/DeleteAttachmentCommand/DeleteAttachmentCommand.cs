using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Attachments.Commands.DeleteAttachmentCommand;

public class DeleteAttachmentCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, Result>
{
    private readonly IApplicationDbContext _context; 
    private readonly IUploadService _uploadService;

    public DeleteAttachmentCommandHandler(IApplicationDbContext context, 
        IUploadService uploadService)
    {
        _context = context;
        _uploadService = uploadService;
    }

    public async Task<Result> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();
         
        var attachments = await _context.Attachments
                    .Where(x => idList.Contains(x.Id))
                    .ToListAsync(cancellationToken);
        _context.Attachments.RemoveRange(attachments);
        var count = await _context.SaveChangesAsync(cancellationToken);
        if (count > 0)
        {
            foreach (var item in attachments)
            {
                _uploadService.DeleteFile(item.Url??"");
            }
        } 

        return count > 0 ? Result.Success() : Result.Failure();

    }
}
