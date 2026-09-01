using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TinyHomeTodo.Api.Swagger;

// implicit [Required] is suppressed app-wide, so Swashbuckle emits an empty required
public class RequiredNonNullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is null)
        {
            return;
        }

        foreach (var (name, property) in schema.Properties)
        {
            if (!property.Nullable && !schema.Required.Contains(name))
            {
                schema.Required.Add(name);
            }
        }
    }
}
