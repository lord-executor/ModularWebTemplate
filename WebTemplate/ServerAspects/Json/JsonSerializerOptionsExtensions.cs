using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebTemplate.ServerAspects.Json;

public static class JsonSerializerOptionsExtensions
{
    public static void RegisterAppConverters(this JsonSerializerOptions options)
    {
        // Convert enums as their string values instead of integer values - comes with System.Text.Json
        options.Converters.Add(new JsonStringEnumConverter());
    }
}
