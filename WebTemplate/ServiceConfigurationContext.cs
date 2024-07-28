namespace WebTemplate;

public record ServiceConfigurationContext(WebApplicationBuilder Builder, IVersionInfo VersionInfo)
{
    public IServiceCollection Services => Builder.Services;
    public ConfigurationManager Configuration => Builder.Configuration;
    public IWebHostEnvironment Environment => Builder.Environment;
}
