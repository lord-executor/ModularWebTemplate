using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;

namespace WebTemplate.ServerAspects.DataProtection;

public class DataProtectionModule : IAppConfigurationModule
{
    public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddDataProtection(options =>
        {
            options.ApplicationDiscriminator = "WebTemplate";
        });

        services.AddTransient<ClusterXmlRepository>();
        services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(provider =>
        {
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = provider.GetRequiredService<ClusterXmlRepository>();
            });
        });
    }

    public void ConfigureApplication(WebApplication app)
    {
    }
}