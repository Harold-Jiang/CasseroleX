using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace CasseroleX.Application.Configurations.Commands.UpdateConfigCommand;
public class UpdateConfigCommand : IRequest<Result>
{
    public required IFormCollection Form { get; set; }
}

public class UpdateConfigCommandHandler : IRequestHandler<UpdateConfigCommand, Result>
{ 
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public UpdateConfigCommandHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result> Handle(UpdateConfigCommand request, CancellationToken cancellationToken)
    {
        if (request.Form.Keys.Count == 0)
            throw new ArgumentNullException();

        //get all configs
        var configs = await _context.SiteConfigurations.ToListAsync(cancellationToken);

        List<SiteConfiguration> eidtSiteConfigurationList = new();

        var groupName = string.Empty;
        foreach (var key in request.Form.Keys)
        {
            if (key.IsNotNullOrEmpty())
            {
                var config = configs.Where(x => x.Name == key).SingleOrDefault();
                if (config == null)
                {
                    continue;
                }
                if (request.Form.TryGetValue(key, out StringValues value))
                {
                    _context.SiteConfigurations.Attach(config);
                    config.Value = value;
                    groupName = config.Group;
                    eidtSiteConfigurationList.Add(config); 
                }
            }
        }

        _context.SiteConfigurations.UpdateRange(eidtSiteConfigurationList);
        var count = await _context.SaveChangesAsync(cancellationToken);

        if (count > 0)
        {
            //remove cache
            await _cache.RemoveAsync(string.Format(CacheKeys.CONFIGURATION_SITE_BY_TYPE_KEY, $"{groupName.ToLowerInvariant()}configinfo"), cancellationToken);
            return Result.Success();
        }

        return Result.Failure();
    }
}