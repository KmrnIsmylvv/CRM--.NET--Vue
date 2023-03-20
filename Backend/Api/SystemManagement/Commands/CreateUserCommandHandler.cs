using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        readonly ISystemUserRepository users;
        readonly ISystemGroupRepository groups;
        readonly ISystemUserGroupAssociationRepository userGroups;
        readonly ISystemUserPermissionAssociationRepository userPermissions;

        public CreateUserCommandHandler(ISystemUserRepository users, ISystemGroupRepository groups,
                                        ISystemUserGroupAssociationRepository userGroups,
                                        ISystemUserPermissionAssociationRepository userPermissions)
        {
            this.users = users;
            this.groups = groups;
            this.userGroups = userGroups;
            this.userPermissions = userPermissions;
        }

        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var ex = await ValidateUsername(command.Username);

            SystemUser user = default;

            ex.CollectErrors(() =>
                user = SystemUser.Create(username: command.Username,
                                         languageCode: command.LanguageCode,
                                         cultureCode: command.CultureCode,
                                         firstName: command.FirstName,
                                         lastName: command.LastName,
                                         email: command.Email,
                                         telephoneNumber: command.TelephoneNumber))
                .TryThrow();

            await users.AddAsync(user);

            foreach (var a in await Associate(user.Id, command.IdGroupList.Select(g => new SystemGroupID(g))))
                await userGroups.AddAsync(a);

            foreach (var a in await Associate(user.Id, command.Permissions.Select(p => p)))
                await userPermissions.AddAsync(a);

            List<SystemUserID> usersList = new List<SystemUserID>();
            List<SystemUserID> usersList2 = new List<SystemUserID>();


            usersList.Intersect(usersList2);

            return user.Id.Value;
        }

        private async Task<List<SystemUserGroupAssociation>> Associate(SystemUserID idUser, IEnumerable<SystemGroupID> idGroupList)
        {
            var associations = new List<SystemUserGroupAssociation>();
            var ex = new ValidationException();

            foreach (var idGroup in idGroupList)
            {
                ex += await ValidateIdGroup(idGroup)
                    + (await ValidateAssociation(new SystemUserGroupAssociationID(idUser, idGroup)))
                        .CollectErrors(() => associations.Add(SystemUserGroupAssociation.Create(idUser, idGroup)));
            }

            ex.TryThrow();

            return associations;
        }

        public async Task<List<SystemUserPermissionAssociation>> Associate(SystemUserID idUser, IEnumerable<string> permissions)
        {
            var associations = new List<SystemUserPermissionAssociation>();
            var ex = new ValidationException();

            foreach (var permission in permissions)
            {
                ex += ValidatePermission(permission)
                    + (await ValidateAssociation(new SystemUserPermissionAssociationID(idUser, permission)))
                        .CollectErrors(() => associations.Add(SystemUserPermissionAssociation.Create(idUser, permission)));
            }

            ex.TryThrow();

            return associations;
        }

        private async Task<ValidationException> ValidateUsername(string username)
        {
            var ex = new ValidationException();

            if (await users.ExistsWithUsername(username))
                ex.AddError(nameof(SystemUser.Username), ValidationErrorCode.UsernameAlreadyExists);

            return ex;
        }

        private async Task<ValidationException> ValidateIdGroup(SystemGroupID id)
        {
            var ex = new ValidationException();

            if (!await groups.ExistsAsync(id))
                ex.AddError("Group", ValidationErrorCode.GroupDoesNotExist);

            return ex;
        }

        private ValidationException ValidatePermission(string permission)
        {
            var ex = new ValidationException();

            if (string.IsNullOrWhiteSpace(permission))
                ex.AddError("Permission", ValidationErrorCode.EmptyPermission);

            return ex;
        }

        private async Task<ValidationException> ValidateAssociation(SystemUserGroupAssociationID id)
        {
            var ex = new ValidationException();

            if (await userGroups.ExistsAsync(id))
                ex.AddError(nameof(SystemUserGroupAssociation), ValidationErrorCode.UserGroupAssociationAlreadyExists);

            return ex;
        }

        private async Task<ValidationException> ValidateAssociation(SystemUserPermissionAssociationID id)
        {
            var ex = new ValidationException();

            if (await userPermissions.ExistsAsync(id))
                ex.AddError(nameof(SystemUserPermissionAssociation), ValidationErrorCode.UserPermissionAssociationAlreadyExists);

            return ex;
        }
    }
}
