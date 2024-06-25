using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace WebTemplate.Status;

public class StatusReporter
{
    public Dictionary<string, object> StatusReport(IServiceProvider serviceProvider)
    {
        var result = new Dictionary<string, object>();
        var providers = serviceProvider.GetServices<IStatusReportProvider>().OrderBy(p => p.Key).ToList();

        //var format = serviceProvider.GetRequiredService<ISecureDataFormat<GitVersionInfo>>();
        // var prot = format.Protect(new GitVersionInfo());
        //var prot = ProtectedData.Protect("ABC"u8.ToArray(), null, DataProtectionScope.LocalMachine);
        var protectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();
        var protector = protectionProvider.CreateProtector("MyThing");
        var data = protector.Protect("ABC"u8.ToArray());

        foreach (var provider in providers)
        {
            result[provider.Key] = provider.StatusReport();
        }

        Console.WriteLine(Encoding.UTF8.GetString(protector.Unprotect(data)));

        return result;
    }
}