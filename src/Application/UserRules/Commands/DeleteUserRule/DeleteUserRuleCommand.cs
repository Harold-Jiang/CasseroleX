using AutoMapper;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.UserRules.Queries;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserRules.Commands.DeleteUserRule;

public class DeleteUserRuleCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteUserRuleCommandHandler : IRequestHandler<DeleteUserRuleCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteUserRuleCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result> Handle(DeleteUserRuleCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();
         
        foreach (var id in idList.ToList())
        {
            var rules = await _context.UserRules
               .Select(s => new UserRuleDto { Id = s.Id, Pid = s.Pid })
               .ToListAsync(cancellationToken);

            var childrenIds = Tree.GetChildrenIds(rules, id, true);
            idList.AddRange(childrenIds);
        }
        idList = idList.Distinct().ToList();

        var count = await _context.UserRules
                    .Where(x => idList.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return count > 0 ? Result.Success() : Result.Failure();

    }
}