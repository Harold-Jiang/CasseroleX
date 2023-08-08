using AutoMapper;
using CasseroleX.Application.Common.Exceptions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Attachments.Commands.UpdateAttachmentCommand;
public class UpdateAttachmentCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string? FileName { get; set; }
    public string? Category { get; set; } 
    public string? ImageWidth { get; set; }
    public string? ImageHeight { get; set; }
    public string? ImageType { get; set; }
    public string? ImageFrames { get; set; }
    public string? FileSize { get; set; }
    public string? MimeType { get; set; }
    public string? Extparam { get; set; }
    public string? UploadTime { get; set; }
    public string? Storage { get; set; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UpdateAttachmentCommand, Attachment>();
        }
    }
}

public class UpdateAttachmentCommandHandler : IRequestHandler<UpdateAttachmentCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAttachmentCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachment = await _context.Attachments.FindAsync(new object?[] { request.Id }, cancellationToken) ?? throw new NotFoundException(nameof(Attachment), request.Id);

        attachment = _mapper.Map(request, attachment);
        _context.Attachments.Update(attachment);
        var count = await _context.SaveChangesAsync(cancellationToken);

        return count > 0 ? Result.Success() : Result.Failure();
    }
}