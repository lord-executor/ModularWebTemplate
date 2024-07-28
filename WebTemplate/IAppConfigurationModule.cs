namespace WebTemplate;

public interface IAppConfigurationModule
{
    void ConfigureServices(ServiceConfigurationContext ctx);
    void ConfigureApplication(ApplicationConfigurationContext ctx);
}
