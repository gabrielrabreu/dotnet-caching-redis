using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DCR.WebApi.Scope.Swagger
{
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        public const string UseCacheHeader = "X-Use-Cache";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = UseCacheHeader,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "bool"
                }
            });
        }

    }
}
