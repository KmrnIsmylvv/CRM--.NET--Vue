using Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using System;
using System.Text;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.Test
{
	[TestClass]
	public class MappingsTest
	{
		private IServiceProvider serviceProvider;

		[TestInitialize]
		public void Initialize()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			serviceProvider = new ServiceCollection()
				.AddSingleton(NHibernateHelper.BuildSessionFactory(configuration))
				.BuildServiceProvider();
		}

		[TestMethod]
		[TestCategory("ManualTest")]
		public void NHibernateMappingsAreOk()
		{
			var sessionFactory = serviceProvider.GetService<ISessionFactory>();

			using (var session = sessionFactory.OpenSession())
			{
				var builder = new StringBuilder();
				var allClassMetadata = session.SessionFactory.GetAllClassMetadata();

				foreach (var entry in allClassMetadata)
				{
					try
					{
						session.CreateCriteria(entry.Value.MappedClass).SetMaxResults(1).List();
					}
					catch (Exception ex)
					{
						builder.AppendLine();
						builder.AppendLine($"*************************************INTO THE CLASS {entry}*************************************");
						builder.AppendLine();
						builder.AppendLine(string.Concat(ex.Message, "\r\n", ex.InnerException, "\r\n", ex.StackTrace, "\r\n", ex.Source, "\r\n", ex.Data, "\r\n", ex.TargetSite));
						builder.AppendLine();
						builder.AppendLine();
						builder.AppendLine();
						builder.AppendLine("************************************************************************************************************************************************************************************************************");
					}
				}

				if (builder.Length > 0)
					throw new Exception(builder.ToString());
			}
		}
	}
}
