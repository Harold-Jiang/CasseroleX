using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Attachments.Queries;
public record GetAttachmentDetailQuery(int Id) : IRequest<Attachment?>;
public class GetAttachmentDetailHandler : IRequestHandler<GetAttachmentDetailQuery, Attachment?>
{
    private readonly IApplicationDbContext _context;

    public GetAttachmentDetailHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Attachment?> Handle(GetAttachmentDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Attachments 
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}

