using AutoMapper;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Mappings;
using CasseroleX.Application.Common.Models;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Admins.Queries;

public class GetAdminLogQueryHandler : IRequestHandler<SearchQuery<AdminLog, AdminLog>, PaginatedList<AdminLog>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAdminLogQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<AdminLog>> Handle(SearchQuery<AdminLog, AdminLog> request, CancellationToken cancellationToken)
    {
        return await _context.AdminLogs
           .Where(request.GetQueryLamda())
           .PaginatedListAsync(request.Page, request.Limit);
    }
}
