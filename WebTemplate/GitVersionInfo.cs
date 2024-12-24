using System.Reflection;

namespace WebTemplate;

public record GitVersionInfo : IVersionInfo
{
    public string AssemblyName { get; }
    public string AssemblyVersion { get; }
    public string Version { get; }
    public string Commit { get; }
    public string CommitDate { get; }
    public string Branch { get; }
    public bool IsDirty { get; }

    public GitVersionInfo()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        AssemblyName = entryAssembly?.GetName().Name ?? "<unknown>";
        AssemblyVersion = entryAssembly?.GetName().Version?.ToString() ?? "<unknown>";
        Version = GitInfo.Tag;
        Commit = GitInfo.CommitHash;
        CommitDate = GitInfo.CommitDate;
        Branch = GitInfo.Branch;
        IsDirty = GitInfo.IsDirty;
    }
}