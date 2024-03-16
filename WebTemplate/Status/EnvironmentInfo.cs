namespace WebTemplate.Status;

public record EnvironmentInfo
{
    public string ApplicationName { get; }
    public string EnvironmentName { get; }
    public string ContentRootPath { get; }
    public string WebRootPath { get; }
    
    public EnvironmentInfo(IWebHostEnvironment env)
    {
        ApplicationName = env.ApplicationName;
        EnvironmentName = env.EnvironmentName;
        ContentRootPath = env.ContentRootPath;
        WebRootPath = env.WebRootPath;
    }
}