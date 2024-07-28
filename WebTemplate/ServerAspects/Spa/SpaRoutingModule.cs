namespace WebTemplate.ServerAspects.Spa;

/// <summary>
/// IMPORTANT: The <see cref="SpaDefaultPageMiddleware"/> _by design_ MUST map ALL previously unmapped routes
/// to the default page which makes this module very susceptible to ordering issues. Any middleware that is registered
/// AFTER the SPA default page middleware is not actually going to get a chance to add anything to the routing process
/// if at the point of the SPA default page middleware the "GetEndpoint" method returns null.
/// </summary>
public class SpaRoutingModule : IAppConfigurationModule
{
	public void ConfigureServices(ServiceConfigurationContext ctx)
	{
	}

	public void ConfigureApplication(ApplicationConfigurationContext ctx)
	{
		ctx.App.UseMiddleware<SpaDefaultPageMiddleware>();
		ctx.App.UseStaticFiles();
	}
}
