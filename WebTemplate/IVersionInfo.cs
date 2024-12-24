namespace WebTemplate;

public interface IVersionInfo
{
    string AssemblyName { get; }
    string AssemblyVersion { get; }
    string Version { get; }
    string Commit { get; }
    string CommitDate { get; }
    string Branch { get; }
    bool IsDirty { get; }
}