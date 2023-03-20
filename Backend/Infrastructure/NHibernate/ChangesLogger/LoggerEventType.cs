using System.ComponentModel;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
	public enum LoggerEventType
	{
		[Description("Inserted")] Inserted,
		[Description("Updated")] Updated,
		[Description("Disabled")] Disabled,
		[Description("Enabled")] Enabled,
		[Description("Deleted")] Deleted
	}
}
