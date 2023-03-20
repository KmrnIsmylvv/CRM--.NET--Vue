using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
	public class LoggerHostedService : IHostedService
	{
		readonly LoggerOptions options;
		readonly ISessionFactory sessionFactory;

		public LoggerHostedService(IOptions<LoggerOptions> options, ISessionFactory sessionFactory)
		{
			this.options = options.Value;
			this.sessionFactory = sessionFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			if (!options.IsActive)
				return Task.CompletedTask;
			
			return EnsureChangesLogTableExists();
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

		private string EnsureChangesLogTableExistsSqlQuery =>
			$@"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'{options.SchemaName}' AND TABLE_NAME = N'{options.TableName}')
				BEGIN
					CREATE TABLE[{options.SchemaName}].[{options.TableName}](
						[id] [uniqueidentifier] NOT NULL,
						[application] [varchar](100) NOT NULL,
						[entity] [varchar](500) NOT NULL,
						[eventType] [varchar](50) NOT NULL,
						[eventOldValue] [varchar](max) NOT NULL,
						[eventNewValue] [varchar](max) NOT NULL,
						[sourceApplication] [varchar](100) NOT NULL,
						[source] [varchar](200) NOT NULL,
						[operationId] [uniqueidentifier] NOT NULL,
						[authorUsername] [nvarchar](200) NOT NULL,
						[eventDateTime] [datetimeoffset] (7) NOT NULL,
					CONSTRAINT[PK_{options.TableName}] PRIMARY KEY CLUSTERED
					(
						[id] ASC
					) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
					) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
				END";

		private async Task EnsureChangesLogTableExists()
		{
			using (var session = sessionFactory.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				await session.CreateSQLQuery(EnsureChangesLogTableExistsSqlQuery)
						 .ExecuteUpdateAsync();

				await transaction.CommitAsync();
			}
		}
	}
}
