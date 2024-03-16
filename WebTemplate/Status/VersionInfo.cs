namespace WebTemplate.Status;

public record VersionInfo
{
    public string AssemblyVersion { get; }
    public string Version { get; }
    public string Commit { get; }
    public string Branch { get; }
    public bool IsDirty { get; }
    
    public VersionInfo()
    {
        AssemblyVersion = typeof(VersionInfo).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        Version = ThisAssembly.Git.Tag;
        Commit = ThisAssembly.Git.Sha;
        Branch = ThisAssembly.Git.Branch;
        IsDirty = ThisAssembly.Git.IsDirty;
    }
}