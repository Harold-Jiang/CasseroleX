using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CasseroleX.Application.Configurations.Queries;
public record GetConfigsQuery : IRequest<List<ConfigListDto>>;

public class GetConfigsQueryHandler : IRequestHandler<GetConfigsQuery, List<ConfigListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetConfigsQueryHandler(
        IApplicationDbContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ConfigListDto>> Handle(GetConfigsQuery request, CancellationToken cancellationToken)
    {
        var siteConfigurations = await _context.SiteConfigurations
               .ProjectTo<ConfigDto>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);

        if (siteConfigurations is null)
            return new();

        //get group config
        var groupDto = siteConfigurations.FirstOrDefault(x=>x.Name.ToLowerInvariant() == "configgroup");
        if (groupDto is null || !groupDto.Value.IsNotNullOrEmpty())
            return new();

        var groupConfigDic = JsonSerializer.Deserialize<Dictionary<string,string>>(groupDto.Value);

        if (!groupConfigDic.IsNotNullOrAny())
            return new();

        var configDict = groupConfigDic.ToDictionary(
            group => group.Key.ToLowerInvariant(),
            group => new ConfigListDto
            {
                Name = group.Key,
                Title = group.Value,
                List = new List<ConfigDto>()
            });

        foreach (var item in siteConfigurations)
        {
            if (configDict.TryGetValue(item.Group.ToLowerInvariant(), out var curlist))
            {
                if (item.Value.IsNotNullOrEmpty() && "select, selects, checkbox, radio".ToIList<string>()!.Contains(item.Type ?? ""))
                {
                    var values = item.Value!.Split(",");
                    // Do something with 'values' if needed
                 }
                curlist.List!.Add(item);
            }
        }

       // Set the 'Active' property using the index from Select method
       var configs = configDict.Values.Select((item, index) => { item.Active = index == 0; return item; }).ToList();

        return configs;

    }
}
