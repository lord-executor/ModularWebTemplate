using System.Text.Json.Serialization;

namespace WebTemplate.ServerAspects.Json;

public class JsonModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // NOTE: This configuration should EXACTLY match the configuration in ControllersModule!
        // There are TWO different JsonOptions classes and two different serializers. This one here is used
        // in the context of minimal APIs.
        ctx.Services.ConfigureHttpJsonOptions(opts =>
        {
            opts.SerializerOptions.RegisterAppConverters();
        });
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
    }
}
