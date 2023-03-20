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
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, short>
    {
        readonly ISystemGroupRepository groups;
        readonly ISystemUserRepository users;
        readonly ISystemUserGroupAssociationRepository userGroups;
        readonly ISystemGroupPermissionAssociationRepository groupPermissions;

        public CreateGroupCommandHandler(ISystemGroupRepository groups, ISystemUserRepository users,
                                         ISystemUserGroupAssociationRepository userGroups,
                                         ISystemGroupPermissionAssociationRepository groupPermissions)
        {
            this.groups = groups;
            this.users = users;
            this.userGroups = userGroups;
            this.groupPermissions = groupPermissions;
        }

        public async Task<short> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
        {
            (await ValidateGroupName(command.Name)).TryThrow();

            var group = SystemGroup.Create(command.Name);

            await groups.AddAsync(group);

            foreach (var a in await Associate(command.IdUserList.Select(u => new SystemUserID(u)), group.Id))
                await userGroups.AddAsync(a);

            foreach (var a in await Associate(group.Id, command.Permissions.Select(p => p)))
                await groupPermissions.AddAsync(a);

            return group.Id.Value;
        }

        private async Task<ValidationException> ValidateGroupName(string name)
        {
            var ex = new ValidationException();

            if (await groups.GroupsWithNameExists(name))
                ex.AddError(nameof(SystemGroup.Name), ValidationErrorCode.GroupAlreadyExistsWithName);

            return ex;
        }

        private async Task<List<SystemUserGroupAssociation>> Associate(IEnumerable<SystemUserID> idUserList, SystemGroupID idGroup)
        {
            (await ValidateIdGroup(idGroup)).TryThrow();

            var associations = new List<SystemUserGroupAssociation>();
            var ex = new ValidationException();

            foreach (var idUser in idUserList)
            {
                ex += await ValidateIdUser(idUser)
                    + (await ValidateAssociation(new SystemUserGroupAssociationID(idUser, idGroup)))
                    .CollectErrors(() => associations.Add(SystemUserGroupAssociation.Create(idUser, idGroup)));
            }

            ex.TryThrow();

            return associations;
        }

        private async Task<List<SystemGroupPermissionAssociation>> Associate(SystemGroupID idGroup, IEnumerable<string> permissions)
        {
            (await ValidateIdGroup(idGroup)).TryThrow();

            if (permissions == null || !permissions.Any())
                throw new ValidationException(nameof(permissions), ValidationErrorCode.GroupShouldHaveAtLeastOnePermission);

            var associations = new List<SystemGroupPermissionAssociation>();
            var ex = new ValidationException();

            foreach (var permission in permissions)
            {
                ex += ValidatePermission(permission)
                    + (await ValidateAssociation(new SystemGroupPermissionAssociationID(idGroup, permission)))
                    .CollectErrors(() => associations.Add(SystemGroupPermissionAssociation.Create(idGroup, permission)));
            }

            ex.TryThrow();

            return associations;
        }

        private async Task<ValidationException> ValidateIdGroup(SystemGroupID id)
        {
            var ex = new ValidationException();

            if (! await groups.ExistsAsync(id))
                ex.AddError("Group", ValidationErrorCode.GroupDoesNotExist);

            return ex;
        }

        private async Task<ValidationException> ValidateIdUser(SystemUserID id)
        {
            var ex = new ValidationException();

            if (!await users.ExistsAsync(id))
                ex.AddError("User", ValidationErrorCode.UserDoesNotExist);

            return ex;
        }

        private async Task<ValidationException> ValidateAssociation(SystemUserGroupAssociationID id)
        {
            var ex = new ValidationException();

            if (await userGroups.ExistsAsync(id))
                ex.AddError(nameof(SystemUserGroupAssociation), ValidationErrorCode.UserGroupAssociationAlreadyExists);

            return ex;
        }

        private ValidationException ValidatePermission(string permission)
        {
            var ex = new ValidationException();

            if (string.IsNullOrWhiteSpace(permission))
                ex.AddError("Permission", ValidationErrorCode.EmptyPermission);

            return ex;
        }

        private async Task<ValidationException> ValidateAssociation(SystemGroupPermissionAssociationID id)
        {
            var ex = new ValidationException();

            if (await groupPermissions.ExistsAsync(id))
                ex.AddError(nameof(SystemGroupPermissionAssociation), ValidationErrorCode.GroupPermissionAssociationAlreadyExists);

            return ex;
        }
    }
}
