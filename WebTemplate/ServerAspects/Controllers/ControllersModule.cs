namespace WebTemplate.ServerAspects.Controllers;

public class ControllersModule : IAppConfigurationModule
{
    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddControllers();
        services.Configure<ApiSettings>(config.GetSection(ApiSettings.SectionName));
    }

    public void ConfigureApplication(WebApplication app)
    {
        app.MapControllers();
    }
}