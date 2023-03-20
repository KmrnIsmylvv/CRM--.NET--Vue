using Elfo.Round.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
	public static class SettingsExtensions
	{
		public static void AddRoundSettings(this IServiceCollection services, IConfiguration configuration)
		{
			services
                .AddRoundSettings()
                .WithMsSqlServer(o =>
                {
                    o.ConnectionString = configuration.GetConnectionString("Db");
                    o.ProfileConnection = true;
                });
        }
    }
}
