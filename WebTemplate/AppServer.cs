using WebTemplate.ServerAspects.Auth;
using WebTemplate.ServerAspects.Cors;
using WebTemplate.ServerAspects.Spa;
using WebTemplate.ServerAspects.Swagger;

namespace WebTemplate;

public class AppServer
{
    private readonly IList<IAppConfigurationModule> _modules = new List<IAppConfigurationModule>
    {
        new AuthModule(),
        new CorsModule(),
        new SpaRoutingModule(),
        new SwaggerModule(),
        //new ApiControllersModule(),
    };

    public void Start(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Configuration.AddJsonFile()

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
}
