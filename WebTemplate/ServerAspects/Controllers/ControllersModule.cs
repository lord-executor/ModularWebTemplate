using WebTemplate.ServerAspects.Json;

namespace WebTemplate.ServerAspects.Controllers;

public class ControllersModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // NOTE: This configuration should EXACTLY match the configuration in JsonModule!
        // There are TWO different JsonOptions classes and two different serializers. This one here is used
        // in the context of MVC controllers.
        ctx.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.RegisterAppConverters();
        });
        ctx.Services.Configure<ApiSettings>(ctx.Configuration.GetSection(ApiSettings.SectionName));
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        ctx.App.MapControllers();
    }
}
