using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebTemplate.ServerAspects.Health;

public class HealthCheckModule : IAppConfigurationModule
{
    private readonly AppStartupHealthCheck _healthCheck = new AppStartupHealthCheck();

    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        ctx.Services.AddHealthChecks()
            .AddCheck(nameof(AppStartupHealthCheck), _healthCheck, HealthStatus.Degraded, ["app"]);
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        ctx.App.MapHealthChecks("/healthz");

        var hostApplicationLifetime = ctx.App.Services.GetRequiredService<IHostApplicationLifetime>();
        hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            _healthCheck.SetStarted();
        });
    }

    private class AppStartupHealthCheck : IHealthCheck
    {
        private HealthCheckResult _result = HealthCheckResult.Degraded("Application still starting up");

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
        {
            return Task.FromResult(_result);
        }

        public void SetStarted()
        {
            _result = HealthCheckResult.Healthy();
        }
    }
}