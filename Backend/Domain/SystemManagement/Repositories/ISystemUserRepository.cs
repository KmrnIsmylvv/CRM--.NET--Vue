using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public interface ISystemUserRepository : IRepository<SystemUser, SystemUserID>
	{
		Task<bool> ExistsWithUsername(string username);
		Task<bool> IsTeacher(SystemUserID idTeacher);
	}
}
