using CasseroleX.Application.Common.Mappings;
using CasseroleX.Domain.Entities;

namespace CasseroleX.Application.Configurations.Queries;
public class ConfigDto :IMapFrom<SiteConfiguration>
{
    public int Id { get; set; }
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
}

public class ConfigListDto
{
    public string? Name { get; set; }
    public string? Title { get; set; }
    public List<ConfigDto>? List { get; set; }
    public bool Active { get; set; }
}


