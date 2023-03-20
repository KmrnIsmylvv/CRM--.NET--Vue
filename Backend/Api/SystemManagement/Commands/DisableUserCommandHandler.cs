using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class DisableUserCommandHandler : IRequestHandler<DisableUserCommand, string>
    {
        readonly ISystemUserRepository users;
        public DisableUserCommandHandler(ISystemUserRepository users) => this.users = users;

        public async Task<string> Handle(DisableUserCommand command, CancellationToken cancellationToken)
        {
            var user = await users.GetAsync(new SystemUserID(command.IdUser))
               ?? throw new ValidationException(nameof(SystemUser), ValidationErrorCode.UserDoesNotExist);
            
            user.Disable();

            return user.Username;
        }
    }
}
