namespace WebTemplate.ServerAspects.Cors;

public class CorsModule : IAppConfigurationModule
{
	public void ConfigureServices(ServiceConfigurationContext ctx)
	{
		var corsSettings = ctx.Configuration.GetRequiredSection(CorsSettings.SectionName).Get<CorsSettings>()!;
        ctx.Services.AddCors(options =>
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

	public void ConfigureApplication(ApplicationConfigurationContext ctx)
	{
		ctx.App.UseCors();
	}
}
