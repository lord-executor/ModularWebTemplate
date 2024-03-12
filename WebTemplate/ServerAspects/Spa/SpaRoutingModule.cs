namespace WebTemplate.ServerAspects.Spa;

public class SpaRoutingModule : IAppConfigurationModule
{
	public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
	{
	}

	public void ConfigureApplication(WebApplication app)
	{
		app.UseMiddleware<SpaDefaultPageMiddleware>();
		app.UseStaticFiles();
	}
}
