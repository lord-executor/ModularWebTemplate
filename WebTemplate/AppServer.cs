using WebTemplate.ServerAspects.Auth;
using WebTemplate.ServerAspects.Controllers;
using WebTemplate.ServerAspects.Cors;
using WebTemplate.ServerAspects.Json;
using WebTemplate.ServerAspects.Spa;
using WebTemplate.ServerAspects.Swagger;
using WebTemplate.Status;

namespace WebTemplate;

public class AppServer
{
    private readonly IList<IAppConfigurationModule> _modules = new List<IAppConfigurationModule>
    {
        new JsonModule(),
        new AuthModule(),
        new CorsModule(),
        new SpaRoutingModule(),
        new SwaggerModule(),
        new StatusEndpointModule(),
        new ControllersModule(),
    };

    public void Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        SetupConfiguration(builder.Configuration, builder.Environment);

        foreach (var appConfigurationModule in _modules)
        {
            appConfigurationModule.ConfigureServices(builder.Services, builder.Configuration);
        }

        var app = builder.Build();
        app.UseHttpsRedirection();

        foreach (var appConfigurationModule in _modules)
        {
            appConfigurationModule.ConfigureApplication(app);
        }

        app.Run();
    }

    private void SetupConfiguration(ConfigurationManager configuration, IWebHostEnvironment env)
    {
        configuration.AddJsonFile($"~/{env.ApplicationName}.json", optional: true);
    }
}
