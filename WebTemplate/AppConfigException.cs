namespace WebTemplate;

public class AppConfigException : Exception
{
    public AppConfigException() { }
    public AppConfigException(string message) : base(message) { }
    public AppConfigException(string message, Exception inner) : base(message, inner) { }
}
