using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserGroups.Queries;

public record GetUserGroupDetailQuery(int Id) : IRequest<UserGroupDto?>;

public class GetUserGroupDetailQueryHandler : IRequestHandler<GetUserGroupDetailQuery, UserGroupDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserGroupDetailQueryHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserGroupDto?> Handle(GetUserGroupDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserGroups
            .ProjectTo<UserGroupDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }

}
