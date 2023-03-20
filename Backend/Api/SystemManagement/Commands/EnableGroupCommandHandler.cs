using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class EnableGroupCommandHandler : IRequestHandler<EnableGroupCommand, Unit>
    {
        readonly ISystemGroupRepository groups;
        public EnableGroupCommandHandler(ISystemGroupRepository groups) => this.groups = groups;

        public async Task<Unit> Handle(EnableGroupCommand command, CancellationToken cancellationToken)
        {
            var group = await groups.GetAsync(new SystemGroupID(command.IdGroup))
                ?? throw new ValidationException(nameof(SystemGroup), ValidationErrorCode.GroupDoesNotExist);

            group.Enable();

            return Unit.Value;
        }
    }
}
