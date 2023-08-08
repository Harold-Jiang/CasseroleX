using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Attachments.Queries;

public class GetAttachmentsQueryHandler : IRequestHandler<SearchQuery<Attachment, AttachmentDto>, PaginatedList<AttachmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUploadService _uploadService;

    public GetAttachmentsQueryHandler(IApplicationDbContext context, IMapper mapper, IUploadService uploadService)
    {
        _context = context;
        _mapper = mapper;
        _uploadService = uploadService;
    }

    public async Task<PaginatedList<AttachmentDto>> Handle(SearchQuery<Attachment, AttachmentDto> request, CancellationToken cancellationToken)
    {
        var attachments = await _context.Attachments
            .Where(request.GetQueryLamda()) 
            .ProjectTo<AttachmentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Page, request.Limit);

        foreach (var item in attachments.Items)
        {
            item.FullUrl =  _uploadService.GetFullUrl(item.Url);
        }
        return attachments;
    }
}
