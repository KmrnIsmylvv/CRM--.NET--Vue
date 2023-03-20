using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.SystemManagement
{
    public class SystemGroupRepository : BaseRepository<SystemGroup, SystemGroupID>, ISystemGroupRepository
    {
        public SystemGroupRepository(ISession session) : base(session) { }

        public async Task<bool> GroupsWithNameExists(string name) =>
            await session.Query<SystemGroup>().CountAsync(g => g.Name.ToLower() == name.ToLower()) > 0;

        public override async Task<bool> ExistsAsync(SystemGroupID id) =>
            await session.Query<SystemGroup>().CountAsync(f => f.Id == id && f.IsEnabled) > 0;
    }
}
