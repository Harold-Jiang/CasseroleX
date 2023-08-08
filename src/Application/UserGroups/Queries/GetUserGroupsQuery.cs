using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserGroups.Queries;
public record GetUserGroupsQuery : IRequest<List<UserGroupDto>>;


public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, List<UserGroupDto>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserGroupsQueryHandler(IMapper mapper, IApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<UserGroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {
         return await _context.UserGroups
            .ProjectTo<UserGroupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
