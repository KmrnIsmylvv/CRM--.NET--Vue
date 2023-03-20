using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elfo.Round.Profiler;
using Elfo.Round.Identity.Impersonation;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class ProfilerExtensions
    {
        public static void AddRoundProfiler(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRoundProfiler(o =>
                {
                    o.Secret = configuration["Profiler:Secret"];
                    o.HowToGetUserIdentityName = (httpRequest) =>
                    {
                        if (!httpRequest.HttpContext.User.Identity.IsAuthenticated)
                            return "";

                        var impersonatedBy = httpRequest.HttpContext.User.Claims.FirstOrDefault(c => c.Type == RoundImpersonationClaimType.ImpersonatedBy);
                        return impersonatedBy != null ? impersonatedBy.Value : httpRequest.HttpContext.User.Identity.Name;
                    };
                });
        }

        public static void AddSwaggerProfiler(this SwaggerGenOptions swaggerGenOptions, IConfiguration configuration)
        {
            swaggerGenOptions.OperationFilter<ProfilerFilter>();
        }
    }

    /// <summary>
    /// Operation filter to add the requirement of the custom header
    /// </summary>
    public class ProfilerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = ProfilerMiddleware.ProfilerHeaderKey,
                In = ParameterLocation.Header,
                Description = "the token needed for profile this request",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("")
                }
            });
        }
    }
}
