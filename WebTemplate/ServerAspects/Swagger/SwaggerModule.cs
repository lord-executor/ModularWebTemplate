using System.Reflection;

namespace WebTemplate.ServerAspects.Swagger;

public class SwaggerModule : IAppConfigurationModule
{
	public void ConfigureServices(IServiceCollection services, IConfigurationRoot config)
	{
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(swaggerGen =>
		{
			swaggerGen.SupportNonNullableReferenceTypes();
			swaggerGen.EnableAnnotations();
			// The StringEnumSchemaFilter changes the representation of enums from numbers to strings in
			// the OpenApi document. This assumes that the JSON serializer has ben configured to use the
			// JsonStringEnumConverter - the two play hand-in-hand.
			swaggerGen.SchemaFilter<StringEnumSchemaFilter>();
			
			// This requires the <GenerateDocumentationFile> property to be set to true in the
			// .csrpoj file
			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
			if (File.Exists(xmlPath))
			{
				swaggerGen.IncludeXmlComments(xmlPath);
			}
		});
	}

	public void ConfigureApplication(WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
	}
}
