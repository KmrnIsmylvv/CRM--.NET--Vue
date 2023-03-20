using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class DisableGroupCommandHandler : IRequestHandler<DisableGroupCommand, Unit>
    {
        readonly ISystemGroupRepository groups;
        public DisableGroupCommandHandler(ISystemGroupRepository groups) => this.groups = groups;

        public async Task<Unit> Handle(DisableGroupCommand command, CancellationToken cancellationToken)
        {
            var group = await groups.GetAsync(new SystemGroupID(command.IdGroup))
                ?? throw new ValidationException(nameof(SystemGroup), ValidationErrorCode.GroupDoesNotExist);

            group.Disable();

            return Unit.Value;
        }
    }
}
