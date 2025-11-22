using System.Reflection;

using Microsoft.OpenApi;

namespace WebTemplate.ServerAspects.Swagger;

public class SwaggerModule : IAppConfigurationModule
{
    public void ConfigureServices(ServiceConfigurationContext ctx)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        ctx.Services.AddEndpointsApiExplorer();
        ctx.Services.AddSwaggerGen(swaggerGen =>
        {
            swaggerGen.SupportNonNullableReferenceTypes();
            swaggerGen.EnableAnnotations();
            // The StringEnumSchemaFilter changes the representation of enums from numbers to strings in
            // the OpenApi document. This assumes that the JSON serializer has been configured to use the
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

    public void ConfigureApplication(ApplicationConfigurationContext ctx)
    {
        // OpenApi documents and the Swagger UI should generally not be exposed in production environments based
        // on the assumption that production environments are _publicly_ accessible. That is not always the case
        // and for somewhat protected scenarios, exposing the API documentation can be very helpful which is why
        // we are not using the default "IsDevelopment" check, but instead provide an environment variable that
        // can be used to disable Swagger when necessary. Undocumented APIs are just not that useful.
        //if (app.Environment.IsDevelopment())
        if (Environment.GetEnvironmentVariable("SVC_DISALBE_SWAGGER") != "1")
        {
            ctx.App.UseSwagger(opt =>
            {
                opt.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
            });
            ctx.App.UseSwaggerUI();
        }
    }
}