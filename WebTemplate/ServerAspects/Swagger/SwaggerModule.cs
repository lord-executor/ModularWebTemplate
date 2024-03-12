namespace WebTemplate.ServerAspects.Swagger;

public class SwaggerModule : IAppConfigurationModule
{
	public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
	{
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	public void ConfigureApplication(WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
	}
}
