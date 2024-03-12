namespace WebTemplate.ServerAspects.Cors;

public class CorsModule : IAppConfigurationModule
{
	public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
	{
		var corsSettings = config.GetRequiredSection(CorsSettings.SectionName).Get<CorsSettings>()!;
		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.WithOrigins(corsSettings.Origins);
				policy.WithHeaders("Authorization", "Content-Type");
				policy.AllowAnyMethod();
				policy.WithExposedHeaders("Content-Disposition");
			});
		});
	}

	public void ConfigureApplication(WebApplication app)
	{
		app.UseCors();
	}
}
