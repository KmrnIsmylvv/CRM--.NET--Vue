using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.SystemManagement
{
	public class SystemUserIDType : CustomUserType<SystemUserID, int>
	{
	}
}
