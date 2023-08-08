using System.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserRules.Queries;
public record GetRuleTreeQuery(List<int>? selected = null) : IRequest<List<UserRuleTreeDto>?>;

public class GetRuleTreeQueryHandler : IRequestHandler<GetRuleTreeQuery, List<UserRuleTreeDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRuleTreeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserRuleTreeDto>?> Handle(GetRuleTreeQuery request, CancellationToken cancellationToken)
    {
        var ruleList = await _context.UserRules
            .Where(r => r.Status == Status.normal)
            .OrderByDescending(r => r.Weigh)
            .ThenByDescending(r => r.Id)
            .ProjectTo<UserRuleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var nodeList = new List<UserRuleTreeDto>();

        ruleList = Tree.GetTreeList(Tree.GetTreeArray(ruleList, 0));

        var hasChildrenIds = ruleList
            .Where(n => n.HasChild == 1)
            .Select(n => n.Id)
            .ToList();

        foreach (var node in ruleList)
        {
            var state = new Dictionary<string, bool> { { "selected", request.selected != null && request.selected.Contains(node.Id) && !hasChildrenIds.Contains(node.Id) } };
            nodeList.Add(new UserRuleTreeDto
            {
                Id = node.Id,
                Parent = node.Pid > 0 ? node.Pid.ToString() : "#",
                Text = node.Title,
                Type = "menu", 
                State = state
            });
        }

        return nodeList;
    }
}

