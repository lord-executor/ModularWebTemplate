namespace WebTemplate.Status;

/// <summary>
/// Overall service status result.
/// </summary>
/// <param name="Status">The value "OK" if everything is OK. Otherwise this service will not actually return a 200
/// HTTP result</param>
/// <param name="Version">Detailed version information</param>
/// <param name="Context">Collection of contextual information provided by different modules</param>
public record ServiceStatus(string Status, IVersionInfo Version, Dictionary<string, object> Context);
