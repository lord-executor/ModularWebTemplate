using Microsoft.Extensions.Diagnostics.HealthChecks;

using WebTemplate.ServerAspects.Auth;
using WebTemplate.ServerAspects.Controllers;
using WebTemplate.ServerAspects.Cors;
using WebTemplate.ServerAspects.Health;
using WebTemplate.ServerAspects.Json;
using WebTemplate.ServerAspects.Spa;
using WebTemplate.ServerAspects.Swagger;
using WebTemplate.ServerAspects.Validation;
using WebTemplate.Status;

namespace WebTemplate;

public class AppServer
{
    private readonly IVersionInfo _versionInfo;

    /// <summary>
    /// These are all the modules that are loaded. The order is _somewhat_ important since in many situations it is
    /// relevant which configuration or middleware is applied first and the modules are applied in the order in
    /// which they appear here.
    /// </summary>
    private readonly IList<IAppConfigurationModule> _modules;

    public AppServer(IVersionInfo versionInfo)
    {
        _versionInfo = versionInfo;
        _modules = new List<IAppConfigurationModule>
        {
            new ProxyAwareModule(),
            new AuthModule(),
            new CorsModule(),
            new JsonModule(),
            new SwaggerModule(),
            new ValidationModule(),
            new HealthCheckModule(),
            new StatusEndpointModule(_ => _versionInfo),
            new ControllersModule(),
            // Keep the SpaRoutingModule at the very end
            new SpaRoutingModule(),
        };
    }

    public void Start(string[] args)
    {
        if (args.Contains("--version"))
        {
            Console.WriteLine(string.IsNullOrEmpty(_versionInfo.Version)
                // no tags available yet, so we fall back to the assembly version
                ? _versionInfo.AssemblyVersion
                : _versionInfo.Version);

            return;
        }

        var builder = WebApplication.CreateBuilder(args);
        SetupConfiguration(builder.Configuration, builder.Environment);

        var serviceConfigCtx = new ServiceConfigurationContext(builder, _versionInfo);
        foreach (var appConfigurationModule in _modules)
        {
            appConfigurationModule.ConfigureServices(serviceConfigCtx);
        }

        var app = builder.Build();
        RegiserStartupLifecycleHook(app);

        // Unfortunately, there is no reasonable way to determine the endpoints that will eventually be used before
        // the server is actually running, at which point it is of course too late to configure HTTPS redirects.
        // This means that we have two separate configurations that we have to keep aligned manually.
        var useHttpsRedirection = app.Configuration.GetSection("UseHttpsRedirection").Get<bool>();
        if (useHttpsRedirection)
        {
            app.UseHttpsRedirection();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        var appConfigCtx = new ApplicationConfigurationContext(app, _versionInfo);
        foreach (var appConfigurationModule in _modules)
        {
            appConfigurationModule.ConfigureApplication(appConfigCtx);
        }

        app.Run();
    }

    private void SetupConfiguration(ConfigurationManager configuration, IWebHostEnvironment env)
    {
        configuration.AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{env.ApplicationName}.json", optional: true);
    }

    /// <summary>
    /// All we really want to do here is log the container name and version after the service has been successfully bootstrapped.
    /// </summary>
    private void RegiserStartupLifecycleHook(WebApplication app)
    {
        // See https://andrewlock.net/finding-the-urls-of-an-aspnetcore-app-from-a-hosted-service-in-dotnet-6/
        var hostApplicationLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            app.Logger.LogInformation("Started {container} {version}", _versionInfo.AssemblyName, _versionInfo.Version);
        });
    }
}
