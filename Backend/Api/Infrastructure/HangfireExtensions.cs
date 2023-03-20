using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.RecurringJobAdmin;
using Microsoft.Data.SqlClient;
using Hangfire.SqlServer;
using System;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public static class HangfireExtensions
    {
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(c => c
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(() => new SqlConnection(configuration.GetConnectionString("HangfireDb")), new SqlServerStorageOptions
                {
                    SchemaName = "hangfire",
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                    //PrepareSchemaIfNecessary = false  //Once db objects are created this may help reducing the number of queries
                })
                .UseRecurringJobAdmin(includeReferences: true, typeof(Startup).Assembly));
        }
    }
}
