namespace WebTemplate.Status;

/// <summary>
/// Overall service status result.
/// </summary>
/// <param name="Status">The value "OK" if everything is OK. Otherwise this service will not actually return a 200
/// HTTP result</param>
/// <param name="Version">Detailed version information</param>
/// <param name="Environment">Collection of environment variables. Only enabled in _Development_ mode</param>
public record ServiceStatus(string Status, VersionInfo Version, EnvironmentInfo Environment);