using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebTemplate.ServerAspects.Swagger;

/// <summary>
/// This is a custom schema filter that updates the OpenApi definition of enums to specify their _string_ values
/// instead of their numeric values and also adds a description from a <see cref="DescriptionAttribute"/> to each
/// value if that attribute is present on the enum.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class StringEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (type.IsEnum)
        {
            schema.Type = "string";
            schema.Format = null;
            schema.Enum = Enum.GetNames(type).Select(name => new OpenApiString(name)).Cast<IOpenApiAny>().ToList();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            var description = new StringWriter();
            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute != null)
                {
                    description.WriteLine($"* `{field.Name}` - {descriptionAttribute.Description}");
                }
            }

            schema.Description = description.ToString();
        }
    }
}
