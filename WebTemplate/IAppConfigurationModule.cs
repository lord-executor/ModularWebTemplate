namespace WebTemplate;

public interface IAppConfigurationModule
{
    void ConfigureServices(IServiceCollection services, IConfigurationRoot config);
    void ConfigureApplication(WebApplication app);
}
