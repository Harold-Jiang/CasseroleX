using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Users.Queries;

public record GetUserDetailQuery(int Id) : IRequest<UserDto?>;

public class GetUserDetailHandler : IRequestHandler<GetUserDetailQuery, UserDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserDetailHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper; 
    }

    public async Task<UserDto?> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
         
        return user;
    }

}
