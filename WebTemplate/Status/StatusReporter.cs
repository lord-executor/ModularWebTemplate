namespace WebTemplate.Status;

public class StatusReporter
{
    public Dictionary<string, object> StatusReport(IServiceProvider serviceProvider)
    {
        var result = new Dictionary<string, object>();
        var providers = serviceProvider.GetServices<IStatusReportProvider>().OrderBy(p => p.Key).ToList();

        foreach (var provider in providers)
        {
            result[provider.Key] = provider.StatusReport();
        }

        return result;
    }
}