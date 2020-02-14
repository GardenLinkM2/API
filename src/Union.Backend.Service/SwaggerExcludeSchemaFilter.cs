using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Union.Backend.Service
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    { }

    public class SwaggerExcludeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var excludedProperties = context.Type.GetProperties()
                .Where(p => p.GetCustomAttribute<SwaggerExcludeAttribute>() != null)
                .ToList();
            foreach (PropertyInfo excludedProperty in excludedProperties)
            {
                var dict = new Dictionary<string, OpenApiSchema>(schema.Properties, StringComparer.InvariantCultureIgnoreCase);
                if (dict.ContainsKey(excludedProperty.Name))
                {
                    dict.Remove(excludedProperty.Name);
                    schema.Properties = dict;
                }
            }
        }
    }
}
