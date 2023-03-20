using Elfo.Round.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class LoggerExtensions
	{
		public static IApplicationBuilder UseLoggingScope(this IApplicationBuilder app)
		{
			return app.UseMiddleware<LoggingScopeMiddleware>();
		}
	}

    public class LoggingScopeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingScopeMiddleware> _logger;

        public LoggingScopeMiddleware(RequestDelegate next, ILogger<LoggingScopeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var dictionary = new Dictionary<string, object>();
            if (context.User.Identity.IsAuthenticated)
                dictionary.Add("UserName", context.User.Identity.Name);

            if (Guid.TryParse(context.Request.Headers[HttpHeaders.OperationId], out Guid operationId))
                dictionary.Add(HttpHeaders.OperationId, operationId);

            using (_logger.BeginScope(dictionary))
            {
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception ex)
                {
                    //If you don't use the Round ExceptionHandlerAttribute (or a inherited attribute) on your controllers,
                    //the unhandled exceptions are caught here. Otherwise they are be caught by the attribute.
                    _logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}