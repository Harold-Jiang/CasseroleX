using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Queries;

public record GetAdminLogDetailQuery(int Id) : IRequest<AdminLog?>;

public class GetAdminLogDetailHandler : IRequestHandler<GetAdminLogDetailQuery, AdminLog?>
{
    private readonly IApplicationDbContext _context;

    public GetAdminLogDetailHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminLog?> Handle(GetAdminLogDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.AdminLogs
            .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
    }

}
