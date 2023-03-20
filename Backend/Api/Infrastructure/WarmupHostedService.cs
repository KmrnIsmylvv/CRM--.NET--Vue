using Microsoft.Extensions.Hosting;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.Infrastructure
{
    public class WarmupHostedService : IHostedService
	{
		readonly ISessionFactory sessionFactory;

		public WarmupHostedService(ISessionFactory sessionFactory) => this.sessionFactory = sessionFactory;

		public Task StartAsync(CancellationToken cancellationToken) => WarmupNHibernate();

		private async Task WarmupNHibernate()
		{
			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				await transaction.CommitAsync();
			}
		}
		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}
