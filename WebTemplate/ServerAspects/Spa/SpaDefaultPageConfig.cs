namespace WebTemplate.ServerAspects.Spa;

public class SpaDefaultPageConfig
{
	public string DefaultPage { get; set; }

	public SpaDefaultPageConfig()
	{
		DefaultPage = "/index.html";
	}
}
