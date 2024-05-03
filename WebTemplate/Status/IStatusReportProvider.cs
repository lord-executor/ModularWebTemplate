namespace WebTemplate.Status;

public interface IStatusReportProvider
{
    public string Key { get; }
    public object StatusReport();
}