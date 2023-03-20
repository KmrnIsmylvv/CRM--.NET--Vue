using Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using System;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate
{
	public static class NHibernateHelper
	{
		public static ISessionFactory BuildSessionFactory(IConfiguration configuration)
		{
			var configurationSettings = new Dictionary<string, string>
			{
				{ "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
				{ "connection.driver_class", "NHibernate.Driver.MicrosoftDataSqlClientDriver" },
				{ "dialect", "NHibernate.Dialect.MsSql2008Dialect" },
				{ "connection.connection_string", configuration.GetConnectionString("Db") },
				{ "connection.release_mode", "auto" },
				{ "default_schema", "dbo" },
				{ "cache.use_query_cache", "false" },
				{ "flushmode", "auto" },
				{ "show_sql", "false" },
				{ "generate_statistics", "true" },
				{ "cache.provider_class", "NHibernate.Cache.HashtableCacheProvider" },
				{ "cache.use_second_level_cache", "false" },
				{ "adonet.batch_size", "10" },
				{ "prepare_sql", "true" }
			};

			var cfg = new Configuration();

            if (Convert.ToBoolean(configuration["ChangesLogger:IsActive"]))
            {
                cfg.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[] { new NHChangesListener() };
                cfg.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] { new NHChangesListener() };
                cfg.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[] { new NHChangesListener() };
            }

            cfg.SetProperties(configurationSettings);
			cfg.AddAssembly("Elfo.Contoso.LearningRoundKamran.Infrastructure");

			return cfg.BuildSessionFactory();
		}
	}
}
