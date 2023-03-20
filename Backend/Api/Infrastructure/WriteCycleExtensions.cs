using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate;
using Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System.Linq;
using System.Reflection;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class WriteCycleExtensions
    {
        public static void AddRoundWriteCycle(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRoundWriteCycle()
                    .WithNHibernate(o =>
                    {
                        o.BuildSessionFactory = () => NHibernateHelper.BuildSessionFactory(configuration);
                        o.OpenSession = sp => sp.GetRequiredService<ISessionFactory>().WithOptions().Interceptor(new NHChangesInterceptor(sp)).OpenSession();
                        o.ProfileConnection = true;
                    })
                    .WithMessages();

            services.AddRepositories();
            services.AddScoped<CourseValidationService>();
            services.AddScoped<ExamValidationService>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            var repositoryInterfaces = Assembly.Load("Elfo.Contoso.LearningRoundKamran.Domain").DefinedTypes.Where(x => x.IsInterface && x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>))).ToList();
            var repositoryImplementations = Assembly.Load("Elfo.Contoso.LearningRoundKamran.Infrastructure").DefinedTypes.Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>))).ToList();

            foreach (var intefaceType in repositoryInterfaces)
                services.Add(new ServiceDescriptor(intefaceType, repositoryImplementations.FirstOrDefault(x => x.GetInterfaces().Contains(intefaceType)), ServiceLifetime.Scoped));
        }
    }
}
