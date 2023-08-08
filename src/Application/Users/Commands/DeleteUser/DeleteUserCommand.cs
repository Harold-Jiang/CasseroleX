using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(string Ids) : IRequest<Result>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context; 
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var idList = request.Ids.ToIList<int>();
        if (!idList.IsNotNullOrAny())
            return Result.Success();

       
        await _context.Users.Where(x => idList.Contains(x.Id))
                    .ExecuteDeleteAsync(cancellationToken);

        var result = await _context.SaveChangesAsync(cancellationToken) > 0;
        return result ? Result.Success() : Result.Failure();

    }
}
