namespace WebTemplate.ServerAspects.Controllers;

public class ControllersModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        ctx.Services.AddControllers();
        ctx.Services.Configure<ApiSettings>(ctx.Configuration.GetSection(ApiSettings.SectionName));
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        ctx.App.MapControllers();
    }
}
