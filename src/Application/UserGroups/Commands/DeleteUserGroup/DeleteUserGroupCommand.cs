using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.UserGroups.Commands.DeleteUserGroup;

public record DeleteUserGroupCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteUserGroupCommandHandler : IRequestHandler<DeleteUserGroupCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserGroupCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

       
        var result = await _context.UserGroups
            .Where(x => idList.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken) > 0;

        return result ? Result.Success() : Result.Failure();

    }
}
