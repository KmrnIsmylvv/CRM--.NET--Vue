using Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class ChangesLoggerExtensions
    {
        public static IServiceCollection AddChangesLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LoggerOptions>(configuration.GetSection("ChangesLogger"));
            services.AddScoped<LoggerService>();
            services.AddHostedService<LoggerHostedService>();

            return services;
        }
    }
}
