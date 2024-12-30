using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebTemplate.ServerAspects.Observability;

public class OpenTelemetryModule : IAppConfigurationModule
{
    private readonly IVersionInfo _versionInfo;

    public OpenTelemetryModule(IVersionInfo versionInfo)
    {
        _versionInfo = versionInfo;
    }

    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // By convention, the OpenTelemetry setup is based on environment variables as configuration. With .NET we
        // can also specify those environment variables as part of the app settings configuration. The two most important
        // configuration values are
        // - OTEL_EXPORTER_OTLP_ENDPOINT: the base URL of your OTLP endpoint,
        // - OTEL_EXPORTER_OTLP_HEADERS: additional headers that should be sent with every request, like AUTHENTICATION
        // See https://opentelemetry.io/docs/languages/sdk-configuration/otlp-exporter/ for more information
        //ctx.Logging.ClearProviders();
        ctx.Logging.AddOpenTelemetry(options =>
        {
            options
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(_versionInfo.AssemblyName))
                .AddOtlpExporter();
        });

        ctx.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(_versionInfo.AssemblyName))
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddOtlpExporter())
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation().AddOtlpExporter());
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
    }
}