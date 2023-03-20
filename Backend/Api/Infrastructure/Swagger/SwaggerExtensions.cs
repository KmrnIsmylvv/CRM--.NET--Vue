using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
	public static class SwaggerExtensions
	{
		public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen(c =>
			{
				const string docVersion = "v1";
				c.SwaggerDoc(docVersion, new OpenApiInfo
				{
					Title = configuration["ApiName"],
					Version = docVersion
				});

				c.CustomSchemaIds(s => s.FullName.Replace("+","."));

				var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML");
				c.IncludeXmlComments(xmlPath);

				c.AddSwaggerAuthentication(configuration);

				c.AddSwaggerProfiler(configuration);

				c.AddSwaggerOperationContext(configuration);
			});
		}

		public static void UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
		{
			app.UseSwagger(c =>
			{
				c.RouteTemplate = "swagger/{documentName}/swagger.json";
			});
			app.UseSwaggerUI(c =>
			{
				c.DocumentTitle = configuration["ApiName"];
				c.RoutePrefix = "swagger";
				c.SwaggerEndpoint("v1/swagger.json", "Specification 1");
				
				switch (configuration["Authentication:Type"])
				{
					case "JWT":
						var assembly = Assembly.GetExecutingAssembly();
						var ns = assembly.GetName().Name;
						c.IndexStream = () => assembly.GetManifestResourceStream($"{ns}.Infrastructure.Swagger.index.html");
						break;
					default:
						break;
				}
			});
		}
	}
}
