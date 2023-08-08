using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Configurations.Commands.DeleteConfigCommand;
public sealed record DeleteConfigCommand(string filedName) : IRequest<Result>;

public class DeleteConfigCommandHandler : IRequestHandler<DeleteConfigCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public DeleteConfigCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(DeleteConfigCommand request, CancellationToken cancellationToken)
    {
        if (!request.filedName.IsNotNullOrEmpty())
            throw new ArgumentNullException(request.filedName);

        var config = await _context.SiteConfigurations
                            .Where(x => x.Name == request.filedName)
                            .FirstOrDefaultAsync(cancellationToken);
        if (config is not null)
        {
            _context.SiteConfigurations.Remove(config);
            await _context.SaveChangesAsync(cancellationToken);
            //remove cache
            await _cache.RemoveAsync(string.Format(CacheKeys.CONFIGURATION_SITE_BY_TYPE_KEY, $"{config.Group.ToLowerInvariant()}configinfo"), cancellationToken);
            return Result.Success();
        }

        return Result.Failure();
    }
}

