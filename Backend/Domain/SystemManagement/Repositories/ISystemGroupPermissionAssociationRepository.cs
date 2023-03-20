using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
	public interface ISystemGroupPermissionAssociationRepository : IRepository<SystemGroupPermissionAssociation, SystemGroupPermissionAssociationID>
	{
		Task<List<SystemGroupPermissionAssociation>> GetByIdGroup(SystemGroupID idGroup);
	}
}
