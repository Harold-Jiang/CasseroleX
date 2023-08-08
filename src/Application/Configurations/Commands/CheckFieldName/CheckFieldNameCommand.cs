using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Configurations.Commands.CheckFieldName;
public record CheckFieldNameCommand(string name) : IRequest<Result>;

public class CheckFieldNameCommandHandler : IRequestHandler<CheckFieldNameCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public CheckFieldNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CheckFieldNameCommand request, CancellationToken cancellationToken)
    { 
        if (!request.name.IsNotNullOrEmpty())
            throw new ArgumentNullException(nameof(request.name));

        var count = await _context.SiteConfigurations.CountAsync(x => x.Name.ToLower() == request.name.ToLower(), cancellationToken);
  
        
        return count > 0 ?  Result.Failure(OperationResult.NAME_EXISTED): Result.Success(null,OperationResult.PASSED_VALIDATION);
    }
}
