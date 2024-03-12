namespace WebTemplate.ServerAspects.Cors;

public class CorsSettings
{
	public const string SectionName = "Cors";

	public string[] Origins { get; set; } = {};
}
