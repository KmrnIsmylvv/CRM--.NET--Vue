using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public interface ISystemUserPermissionAssociationRepository : IRepository<SystemUserPermissionAssociation, SystemUserPermissionAssociationID>
	{
		Task<List<SystemUserPermissionAssociation>> GetByIdUser(SystemUserID idUser);
	}
}
