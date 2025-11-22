namespace WebTemplate.Status;

public class StatusEndpointModule : IAppConfigurationModule
{
    private readonly Func<IServiceProvider, IVersionInfo> _versionProvider;

    public StatusEndpointModule(Func<IServiceProvider, IVersionInfo> versionProvider)
    {
        _versionProvider = versionProvider;
    }

    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        ctx.Services.AddTransient<IVersionInfo>(_versionProvider);
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        var reporter = new StatusReporter();

        ctx.App.MapGet("v1/status", (IVersionInfo version, IServiceProvider serviceProvider) => new ServiceStatus("OK", version, reporter.StatusReport(serviceProvider)))
            .WithName("StatusService")
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Description =
                    """
                    Returns the overall service status, including version and context information. This can be used
                    as a health check endpoint.
                    """;

                return Task.CompletedTask;
            });
    }
}
