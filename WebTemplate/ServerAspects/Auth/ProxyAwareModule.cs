using Microsoft.AspNetCore.HttpOverrides;

namespace WebTemplate.ServerAspects.Auth;

/// <summary>
/// This module just enables the <see cref="ForwardedHeadersMiddleware"/> which is REQUIRED when the service is sitting
/// behind a reverse proxy and is working with third party authentication providers that use redirects to actually
/// build the right redirect URIs. It has to be implemented as a standalone module because it needs to be one of the
/// first middlewares to get registered so that all subsequent middlewares get the correct host and scheme.
///
/// See https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-8.0 which
/// doesn't explain any of the intricacies mentioned here but is the official documentation of this topic.
/// </summary>
public class ProxyAwareModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        ctx.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            // We explicitly DO NOT forward the ForwardedHeaders.XForwardedFor header because:
            // - Enabling that will actually VALIDATE the proxy IP in ForwardedHeadersMiddleware
            // - The default configuration only includes the IP range 127.0.0.0/8 as valid proxy sources
            // - While that can be manually configured here with options.KnownNetworks and options.KnownProxies
            //   this would be fairly cumbersome and environment specific and probably also not very reliable (e.g.
            //   when working with Kubernetes)
            options.ForwardedHeaders = ForwardedHeaders.All & ~ForwardedHeaders.XForwardedFor;
        });
    }

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        ctx.App.UseForwardedHeaders();
        // Similar to the AuthModule where we have to add the auth middleware EXPLICITLY so that we have a well-defined
        // order, we have to do the same with the EndpointRoutingMiddleware. If we don't explicitly add that middleware
        // here, then it will be autoMAGICALLY prepended and run BEFORE the ForwardedHeadersMiddleware which means that
        // routing will NOT be using the corrected host which means that host based routing will not work.
        ctx.App.UseRouting();
    }
}
