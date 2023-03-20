using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement
{
    public interface ISystemGroupRepository : IRepository<SystemGroup, SystemGroupID>
    {
        Task<bool> GroupsWithNameExists(string name);
    }
}
