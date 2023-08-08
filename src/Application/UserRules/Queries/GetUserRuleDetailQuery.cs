using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserRules.Queries;

public record GetUserRuleDetailQuery(int Id) : IRequest<UserRuleDto?>;
public class GetUserRuleDetailHandler : IRequestHandler<GetUserRuleDetailQuery, UserRuleDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserRuleDetailHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserRuleDto?> Handle(GetUserRuleDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.UserRules
            .ProjectTo<UserRuleDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }

}
