using Elfo.Round.Context;
using Microsoft.Extensions.Options;
using NHibernate;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
    public class LoggerService
    {
        readonly LoggerOptions options;
        readonly ISession session;
        readonly IContextInfoProvider contextInfoProvider;

        public LoggerService(IOptions<LoggerOptions> options, ISession session, IContextInfoProvider contextInfoProvider)
        {
            this.options = options.Value;
            this.session = session;
            this.contextInfoProvider = contextInfoProvider;
        }
        
        public virtual Task LogEntityChanges(string entity, LoggerEventType eventType, string eventOldValue, string eventNewValue)
        {
            if (!options.IsActive)
                return Task.CompletedTask;

            return Log(options.ApplicationName, entity, eventType, eventOldValue, eventNewValue, contextInfoProvider.SourceApplication,
                contextInfoProvider.Source, contextInfoProvider.OperationId, contextInfoProvider.User);
        }

        protected virtual string StoreLogSqlQuery => $"INSERT INTO [{options.SchemaName}].[{options.TableName}] ([id], [application], [entity], [eventType], [eventOldValue], [eventNewValue], [sourceApplication], [source], [operationId], [authorUsername], [eventDateTime]) VALUES (:id, :application, :entity, :eventType, :eventOldValue, :eventNewValue, :sourceApplication, :source, :operationId, :authorUsername, :eventDateTime)";

        protected virtual Task Log(string application, string entity, LoggerEventType eventType, string eventOldValue, string eventNewValue,
            string sourceApplication, string source, Guid operationId, string authorUsername) =>
            session.CreateSQLQuery(StoreLogSqlQuery)
                .SetGuid("id", Guid.NewGuid())
                .SetString("application", application)
                .SetString("entity", entity)
                .SetString("eventType", GetEventTypeDescription(eventType))
                .SetParameter("eventOldValue", eventOldValue, NHibernateUtil.StringClob)
                .SetParameter("eventNewValue", eventNewValue, NHibernateUtil.StringClob)
                .SetString("sourceApplication", sourceApplication)
                .SetString("source", source)
                .SetGuid("operationId", operationId)
                .SetString("authorUsername", authorUsername)
                .SetDateTimeOffset("eventDateTime", DateTimeOffset.UtcNow)
                .ExecuteUpdateAsync();

		public string GetEventTypeDescription(Enum auditType)
		{
			DescriptionAttribute[] attributes = auditType
				.GetType()
				.GetField(auditType.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

			if (attributes != null && attributes.Any())
				return attributes.First().Description;

			return auditType.ToString();
		}
    }
}
