using Elfo.Round.Identity;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class RemoveUeUserFromCacheCommandHandler : IRequestHandler<RemoveUeUserFromCacheCommand, Unit>
    {
        readonly IUserCacheManager userCacheManager;
        public RemoveUeUserFromCacheCommandHandler(IUserCacheManager userCacheManager) => this.userCacheManager = userCacheManager;

        public Task<Unit> Handle(RemoveUeUserFromCacheCommand command, CancellationToken cancellationToken)
        {
            var ueCacheKey = userCacheManager.GetKeys().FirstOrDefault(k => k.StartsWith("ue_"));

            if (!string.IsNullOrEmpty(ueCacheKey))
                userCacheManager.Remove(ueCacheKey);

            return Task.FromResult(Unit.Value);
        }
    }
}
