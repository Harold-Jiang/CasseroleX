using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Admins.Commands.DeleteAdminLog;

public class DeleteAdminLogCommand : IRequest<Result>
{
    public string? Action { get; set; }
    public string? Ids { get; set; }
    public string? Params { get; set; }

}
public class DeleteAdminLogCommandHandler : IRequestHandler<DeleteAdminLogCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteAdminLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteAdminLogCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

        var count = await _context.AdminLogs
                    .Where(x => idList.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken);

        return count > 0 ? Result.Success() : Result.Failure();

    }
}