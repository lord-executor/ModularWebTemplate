﻿using WebTemplate.ServerAspects.Auth;
using WebTemplate.ServerAspects.Controllers;
using WebTemplate.ServerAspects.Cors;
using WebTemplate.ServerAspects.Json;
using WebTemplate.ServerAspects.Spa;
using WebTemplate.ServerAspects.Swagger;
using WebTemplate.ServerAspects.Validation;
using WebTemplate.Status;

namespace WebTemplate;

public class AppServer
{
    /// <summary>
    /// These are all the modules that are loaded. The order is _somewhat_ important since in many situations it is
    /// relevant which configuration or middleware is applied first and the modules are applied in the order in
    /// which they appear here.
    /// </summary>
    private readonly IList<IAppConfigurationModule> _modules = new List<IAppConfigurationModule>
    {
        new AuthModule(),
        new CorsModule(),
        new JsonModule(),
        new SwaggerModule(),
        new ValidationModule(),
        new StatusEndpointModule(_ => new GitVersionInfo()),
        new ControllersModule(),
        // Keep the SpaRoutingModule at the very end
        new SpaRoutingModule(),
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

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        foreach (var appConfigurationModule in _modules)
        {
            appConfigurationModule.ConfigureApplication(app);
        }

        app.Run();
    }

    private void SetupConfiguration(ConfigurationManager configuration, IWebHostEnvironment env)
    {
        configuration.AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{env.ApplicationName}.json", optional: true);
    }
}
