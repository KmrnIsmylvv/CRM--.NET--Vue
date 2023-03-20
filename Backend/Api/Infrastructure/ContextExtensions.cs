using Elfo.Round.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class ContextExtensions
    {
        public static IServiceCollection AddRoundContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRoundContext()
                .ForHttp(o => o.ApplicationName = configuration["ApiName"]);
            
            return services;
        }

        public static void AddSwaggerOperationContext(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
        {
            swaggerGenOptions.OperationFilter<OperationIdFilter>();
        }
    }

    /// <summary>
    /// Operation filter to add the requirement of the custom header
    /// </summary>
    public class OperationIdFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = HttpHeaders.OperationId,
                In = ParameterLocation.Header,
                Description = $"the {HttpHeaders.OperationId} associated to this request",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(""),
                }
            });
        }
    }
}
