using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BatchFileWebAPI.Utility
{
    public class CustomHeaderSwaggerAttribute: IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            if(context.MethodInfo.Name=="AddFile")
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-MIME-Type",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Content-Size",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer"
                    }
                });

            }
        }
    }
}
