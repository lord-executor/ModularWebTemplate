namespace WebTemplate;

public record ApplicationConfigurationContext(WebApplication App, IVersionInfo VersionInfo)
{
    public IServiceProvider ServiceProvider => App.Services;
    public IConfiguration Configuration => App.Configuration;
    public IWebHostEnvironment Environment => App.Environment;
}
