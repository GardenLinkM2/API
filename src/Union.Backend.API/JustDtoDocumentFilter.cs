using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Union.Backend.Model.Models;

namespace Union.Backend.API
{
    public class JustDtoDocumentFilter : IDocumentFilter
    {
        private readonly List<string> necessarySchemas = new List<string>
        {
            nameof(Status),
            nameof(LeasingStatus),
        };

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var schemas = swaggerDoc.Components.Schemas;
            foreach (var kv in schemas)
            {
                if (!necessarySchemas.Contains(kv.Key) && !kv.Key.ToLower().Contains("dto"))
                    schemas.Remove(kv.Key);
            }
            schemas.Add("GardenODataQueryOptions", new OpenApiSchema
            {
                Description = "Prevent Swagger Exception linked to OData using on Garden Entity"
            });
        }
    }
}
