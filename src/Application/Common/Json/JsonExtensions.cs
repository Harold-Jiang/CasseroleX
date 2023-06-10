using System.Text.Json;
using System.Text.Json.Serialization;

namespace CasseroleX.Application.Common.Json;
public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions;

    static JsonExtensions()
    {
        _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = new LowercaseNamingPolicy(),
            DictionaryKeyPolicy = new LowercaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
        };
        _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
    public static string ToJson(this object? obj)
    {
        return obj == null ? string.Empty : JsonSerializer.Serialize(obj, _jsonSerializerOptions);
    }
     
    public static T? ToObject<T>(this string? jsonStr)
    {
        try
        {
            return jsonStr == null ? default : JsonSerializer.Deserialize<T>(jsonStr, _jsonSerializerOptions);
        }
        catch (JsonException)
        {
            return default;
        }
    }
}
