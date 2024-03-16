namespace WebTemplate.Status;

public class StatusEndpointModule : IAppConfigurationModule
{
    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
    }

    public void ConfigureApplication(WebApplication app)
    {
        app.MapGet("/v1/status", () => new ServiceStatus("OK", new VersionInfo(), new EnvironmentInfo(app.Environment)))
            .WithName("StatusService")
            .WithOpenApi(operation =>
            {
                operation.Description =
                    """
                    Returns the overall service status, including version and environment information. This can be used
                    as a health check endpoint.
                    """;

                return operation;
            });
    }
}