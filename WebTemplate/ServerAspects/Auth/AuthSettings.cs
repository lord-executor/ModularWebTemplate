namespace WebTemplate.ServerAspects.Auth;

public class AuthSettings
{
	public const string SectionName = "Auth";

	public string Authority { get; set; } = String.Empty;
	public string Audience { get; set; } = String.Empty;
	public IDictionary<string, string[]> PolicyClaims { get; set; } = new Dictionary<string, string[]>();
}
