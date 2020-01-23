using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Union.Backend.API
{
    public class JustDtoDocumentFilter : IDocumentFilter
    {

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var schemas = swaggerDoc.Components.Schemas;
            foreach (var kv in schemas)
            {
                if (!kv.Key.ToLower().Contains("dto"))
                    schemas.Remove(kv.Key);
            }
        }
    }
}
