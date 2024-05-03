namespace WebTemplate.Status;

public class StatusEndpointModule : IAppConfigurationModule
{
    private readonly Func<IServiceProvider, IVersionInfo> _versionProvider;

    public StatusEndpointModule(Func<IServiceProvider, IVersionInfo> versionProvider)
    {
        _versionProvider = versionProvider;
    }
    
    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddTransient<IVersionInfo>(_versionProvider);
    }

    public void ConfigureApplication(WebApplication app)
    {
        var reporter = new StatusReporter();

        app.MapGet("v1/status", (IVersionInfo version, IServiceProvider serviceProvider) => new ServiceStatus("OK", version, reporter.StatusReport(serviceProvider)))
            .WithName("StatusService")
            .WithOpenApi(operation =>
            {
                operation.Description =
                    """
                    Returns the overall service status, including version and context information. This can be used
                    as a health check endpoint.
                    """;

                return operation;
            });
    }
}
