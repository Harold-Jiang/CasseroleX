using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Roles.Queries;
public record GetRoleDetailQuery(int Id) : IRequest<RoleDto?>;
public class GetRoleDetailHandler : IRequestHandler<GetRoleDetailQuery, RoleDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRoleDetailHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }

}
