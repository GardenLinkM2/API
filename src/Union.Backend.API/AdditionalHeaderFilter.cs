using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;
using Union.Backend.Service.Auth;

namespace Union.Backend.API
{
    public class AdditionalHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = context
                .ApiDescription.ActionDescriptor.FilterDescriptors
                .Select(filterInfo => filterInfo.Filter)
                .Where(f => f is AuthorizeAttribute)
                .Select(f => (AuthorizeAttribute)f)
                .ToList();
            if (authorizeAttributes.Count() == 0 || !authorizeAttributes.OrderBy(a => a.Order).First().Permission.Equals(PermissionType.All))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HttpRequestHeader.Authorization.ToString(),
                    In = ParameterLocation.Header,
                    Description = "access token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString("Bearer {token}")
                    }
                });
            }
        }
    }
}
