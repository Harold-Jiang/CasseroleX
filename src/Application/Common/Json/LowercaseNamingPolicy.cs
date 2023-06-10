using System.Text.Json;

namespace CasseroleX.Application.Common.Json;

public class LowercaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) =>
        name.ToLower();
}
