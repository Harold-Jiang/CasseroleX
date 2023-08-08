using AutoMapper;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Common.Models;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities;
using MediatR;

namespace CasseroleX.Application.Configurations.Commands.CreateConfigCommand;
public class CreateConfigCommand : IRequest<Result>
{
    public string Name { get; set; } = null!;

    public string Group { get; set; } = null!;

    public string? Title { get; set; }

    public string? Tip { get; set; }

    public string? Type { get; set; }

    public string? Visible { get; set; }

    public string? Value { get; set; }

    public string? Content { get; set; }

    public string? Rule { get; set; }

    public string? Extend { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateConfigCommand, SiteConfiguration>(); 
        }
    }
}

public class CreateConfigCommandHandler : IRequestHandler<CreateConfigCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public CreateConfigCommandHandler(IApplicationDbContext context,
        IMapper mapper,
        ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result> Handle(CreateConfigCommand request, CancellationToken cancellationToken)
    {
        if (!"select, selects, checkbox, radio".ToIList<string>()!.Contains(request.Type ?? ""))
            request.Content = string.Empty;

        var model = _mapper.Map<SiteConfiguration>(request);

        await _context.SiteConfigurations.AddAsync(model, cancellationToken);
        var count = await _context.SaveChangesAsync(cancellationToken);

        if (count > 0)
        {
            //remove cache
            await _cache.RemoveAsync(string.Format(CacheKeys.CONFIGURATION_SITE_BY_TYPE_KEY, $"{request.Group.ToLowerInvariant()}configinfo"), cancellationToken);
            return Result.Success();
        }
        return Result.Failure();
    }

}