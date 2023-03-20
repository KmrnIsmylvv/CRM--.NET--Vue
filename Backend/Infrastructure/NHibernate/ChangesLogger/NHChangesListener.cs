using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Round.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
    public class NHChangesListener : IPostInsertEventListener, IPostUpdateEventListener, IPostDeleteEventListener
	{
		private const string IsEnabled = "IsEnabled";

		public void OnPostInsert(PostInsertEvent @event) =>
			LogInsert(@event, CancellationToken.None).GetAwaiter().GetResult();

		public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken) =>
			LogInsert(@event, cancellationToken);

		private async Task LogInsert(PostInsertEvent @event, CancellationToken cancellationToken)
		{
			var entity = @event.Entity as ILoggable;
			if (entity != null)
			{
				await Log(eventSource: @event.Session,
						  entity: entity,
						  eventType: LoggerEventType.Inserted,
						  eventOldValue: string.Empty,
						  eventNewValue: JsonConvert.SerializeObject(entity, LogSerialization.Settings));
			}
		}

		public void OnPostUpdate(PostUpdateEvent @event) =>
			LogUpdate(@event, CancellationToken.None).GetAwaiter().GetResult();

		public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken) =>
			LogUpdate(@event, cancellationToken);

		private async Task LogUpdate(PostUpdateEvent @event, CancellationToken cancellationToken)
		{
			var entity = @event.Entity as ILoggable;
			if (entity != null)
			{
				var eventType = LoggerEventType.Updated;

				int indexIsEnabled = Array.IndexOf(@event.Persister.PropertyNames, IsEnabled);

				if (indexIsEnabled >= 0)
				{
					bool oldValueIsEnabled = (bool)@event.OldState[indexIsEnabled];
					bool newValueIsEnabled = (bool)@event.State[indexIsEnabled];

					if (oldValueIsEnabled != newValueIsEnabled)
						if (newValueIsEnabled)
							eventType = LoggerEventType.Enabled;
						else
							eventType = LoggerEventType.Disabled;
				}

				var dirtyFieldIndexes = await @event.Persister.FindDirtyAsync(@event.State, @event.OldState, @event.Entity, @event.Session, cancellationToken);

				var oldValueObj = new JObject();
				var newValueObj = new JObject();
				foreach (var dirtyFieldIndex in dirtyFieldIndexes)
				{
					var propName = @event.Persister.PropertyNames[dirtyFieldIndex];
					var oldValue = @event.OldState[dirtyFieldIndex];
					var newValue = @event.State[dirtyFieldIndex];

					if (oldValue == newValue)
						continue;

					oldValueObj.Add(propName, oldValue == null ? JValue.CreateNull() : JToken.FromObject(oldValue));
					newValueObj.Add(propName, newValue == null ? JValue.CreateNull() : JToken.FromObject(newValue));
				}

				await Log(eventSource: @event.Session,
						  entity: entity,
						  eventType: eventType,
						  eventOldValue: JsonConvert.SerializeObject(oldValueObj, LogSerialization.Settings),
						  eventNewValue: JsonConvert.SerializeObject(newValueObj, LogSerialization.Settings));
			}
		}

		public void OnPostDelete(PostDeleteEvent @event) =>
			LogDelete(@event, CancellationToken.None).GetAwaiter().GetResult();

		public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken) =>
			LogDelete(@event, cancellationToken);

		private async Task LogDelete(PostDeleteEvent @event, CancellationToken cancellationToken)
		{
			var entity = @event.Entity as ILoggable;
			if (entity != null)
			{
				await Log(eventSource: @event.Session,
						  entity: entity,
						  eventType: LoggerEventType.Deleted,
						  eventOldValue: string.Empty,
						  eventNewValue: JsonConvert.SerializeObject(entity, LogSerialization.Settings));
			}
		}

		private async Task Log(IEventSource eventSource, ILoggable entity, LoggerEventType eventType, string eventOldValue, string eventNewValue)
		{
			using (ISession session = eventSource.SessionWithOptions().Connection().OpenSession())
			{
				var options = ((NHChangesInterceptor)eventSource.Interceptor)
					.ServiceProvider
					.GetRequiredService<IOptions<LoggerOptions>>();

				var contextInfoProvider = ((NHChangesInterceptor)eventSource.Interceptor)
					.ServiceProvider
					.GetRequiredService<IContextInfoProvider>();

				//Do not resolve AuditLoggerService with DI in this listener because it's required to use a child session provided manually
				var logger = new LoggerService(options, session, contextInfoProvider);

				await logger.LogEntityChanges(entity: entity.ShortDescription(),
											  eventType: eventType,
											  eventOldValue: eventOldValue,
											  eventNewValue: eventNewValue);

				foreach (var collection in eventSource.PersistenceContext.CollectionEntries.Values)
				{
					var collectionEntry = collection as CollectionEntry;
					if (collectionEntry != null)
						collectionEntry.IsProcessed = true;
				}

				await session.FlushAsync();
			}
		}

		#region Private classes
		class LogSerialization
		{
			public static JsonSerializerSettings Settings
			{
				get
				{
					return new JsonSerializerSettings
					{
						TypeNameHandling = TypeNameHandling.Auto,
						ContractResolver = new LogContractResolver(),
						ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					};
				}
			}
		}

		class LogContractResolver : DefaultContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var prop = base.CreateProperty(member, memberSerialization);
				if (!prop.Writable)
				{
					var property = member as PropertyInfo;
					var hasPrivateSetter = property?.GetSetMethod(true) != null;
					prop.Writable = hasPrivateSetter;
				}
				return prop;
			}
		}
		#endregion
	}
}
