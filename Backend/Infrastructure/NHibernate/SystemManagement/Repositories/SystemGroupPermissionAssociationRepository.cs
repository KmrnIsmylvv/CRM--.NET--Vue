using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using NHibernate;
using NHibernate.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.SystemManagement
{
	public class SystemGroupPermissionAssociationRepository : BaseRepository<SystemGroupPermissionAssociation, SystemGroupPermissionAssociationID>, ISystemGroupPermissionAssociationRepository
	{
		public SystemGroupPermissionAssociationRepository(ISession session) : base(session) { }

		public Task<List<SystemGroupPermissionAssociation>> GetByIdGroup(SystemGroupID idGroup) =>
			session.Query<SystemGroupPermissionAssociation>().Where(a => a.Id.IdGroup == idGroup).ToListAsync();
	}
}
