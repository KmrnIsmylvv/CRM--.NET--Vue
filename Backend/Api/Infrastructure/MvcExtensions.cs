using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class MvcExtensions
    {
        public static void AddRoundMvc(this IServiceCollection services)
        {
            services.AddHostedService<WarmupHostedService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
