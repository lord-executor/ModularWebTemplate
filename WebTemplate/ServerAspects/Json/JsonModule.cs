using System.Text.Json.Serialization;

namespace WebTemplate.ServerAspects.Json;

public class JsonModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        ctx.Services.ConfigureHttpJsonOptions(opts =>
        {
            // Convert enums as their string values instead of integer values
            var enumConverter = new JsonStringEnumConverter();
            opts.SerializerOptions.Converters.Add(enumConverter);
        });
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
    }
}
