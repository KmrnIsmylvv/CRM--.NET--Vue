using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public interface ISystemUserGroupAssociationRepository : IRepository<SystemUserGroupAssociation, SystemUserGroupAssociationID>
	{
		Task<List<SystemUserGroupAssociation>> GetByIdUser(SystemUserID idUser);
		Task<List<SystemUserGroupAssociation>> GetByIdGroup(SystemGroupID idGroup);
	}
}
