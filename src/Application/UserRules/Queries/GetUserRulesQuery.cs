using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserRules.Queries;
public record GetUserRulesQuery : IRequest<List<UserRuleDto>>;
public class GetUserRulesHandler : IRequestHandler<GetUserRulesQuery, List<UserRuleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserRulesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserRuleDto>> Handle(GetUserRulesQuery request, CancellationToken cancellationToken)
    {
        //get all user rule
        var ruleList = await _context.UserRules 
            .OrderByDescending(x => x.Weigh) 
            .ProjectTo<UserRuleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        if (ruleList is not null)
        { 
            return Tree.GetTreeList(Tree.GetTreeArray(ruleList, 0), "Title");
        }

        return new();
    }
}
