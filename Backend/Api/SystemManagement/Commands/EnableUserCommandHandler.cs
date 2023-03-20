using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class EnableUserCommandHandler : IRequestHandler<EnableUserCommand, string>
    {
        readonly ISystemUserRepository users;
        public EnableUserCommandHandler(ISystemUserRepository users) => this.users = users;

        public async Task<string> Handle(EnableUserCommand command, CancellationToken cancellationToken)
        {
            var user = await users.GetAsync(new SystemUserID(command.IdUser))
               ?? throw new ValidationException(nameof(SystemUser), ValidationErrorCode.UserDoesNotExist);
            
            user.Enable();

            return user.Username;
        }
    }
}
