using Elfo.Round.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class ClearUsersCacheCommandHandler : IRequestHandler<ClearUsersCacheCommand, Unit>
    {
        readonly IUserCacheManager userCacheManager;
        public ClearUsersCacheCommandHandler(IUserCacheManager userCacheManager) => this.userCacheManager = userCacheManager;

        public Task<Unit> Handle(ClearUsersCacheCommand comand, CancellationToken cancellationToken)
        {
            userCacheManager.Clear();
            return Task.FromResult(Unit.Value);
        }
    }
}
