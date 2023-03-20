using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elfo.Round;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Unit>
    {
        readonly ISystemGroupRepository groups;
        readonly ISystemUserRepository users;
        readonly ISystemUserGroupAssociationRepository userGroups;
        readonly ISystemGroupPermissionAssociationRepository groupPermissions;

        public UpdateGroupCommandHandler(ISystemGroupRepository groups, ISystemUserRepository users,
                                         ISystemUserGroupAssociationRepository userGroups,
                                         ISystemGroupPermissionAssociationRepository groupPermissions)
        {
            this.groups = groups;
            this.users = users;
            this.userGroups = userGroups;
            this.groupPermissions = groupPermissions;
        }

        public async Task<Unit> Handle(UpdateGroupCommand command, CancellationToken cancellationToken)
        {
            var group = (await groups.GetAsync(new SystemGroupID(command.IdGroup)))
                ?? throw new ValidationException("IdGroup", ValidationErrorCode.GroupDoesNotExist);

            if (!string.Equals(group.Name, command.Name, StringComparison.InvariantCultureIgnoreCase))
                (await ValidateGroupName(command.Name)).TryThrow();

            group.Update(command.Name);

            var userAssociations = await userGroups.GetByIdGroup(group.Id);

            var (usersToRemove, usersToAdd) = userAssociations.Select(a => a.Id.IdUser).ToList()
                .Differences(command.IdUserList.Select(u => new SystemUserID(u)).ToList());

            foreach (var a in (await Associate(usersToAdd, group.Id)))
                await userGroups.AddAsync(a);

            usersToRemove.Select(u => new SystemUserGroupAssociationID(u, group.Id)).ToList()
                .ForEach(userGroups.Remove);

            var permissionAssociations = await groupPermissions.GetByIdGroup(group.Id);

            var (permissionsToRemove, permissionsToAdd) = permissionAssociations.Select(a => a.Id.Permission).ToList()
                .Differences(command.Permissions.Select(p => p).ToList());

            if (permissionsToAdd.Any())
                foreach (var a in (await Associate(group.Id, permissionsToAdd)))
                    await groupPermissions.AddAsync(a);

            permissionsToRemove.Select(p => new SystemGroupPermissionAssociationID(group.Id, p)).ToList()
                .ForEach(groupPermissions.Remove);

            return Unit.Value;
        }

        private async Task<ValidationException> ValidateGroupName(string name)
        {
            var ex = new ValidationException();

            if (await groups.GroupsWithNameExists(name))
                ex.AddError(nameof(SystemGroup.Name), ValidationErrorCode.GroupAlreadyExistsWithName);

            return ex;
        }

        public async Task<List<SystemUserGroupAssociation>> Associate(IEnumerable<SystemUserID> idUserList, SystemGroupID idGroup)
        {
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

        public async Task<List<SystemGroupPermissionAssociation>> Associate(SystemGroupID idGroup, IEnumerable<string> permissions)
        {
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

        private async Task<ValidationException> ValidateIdUser(SystemUserID id)
        {
            var ex = new ValidationException();

            if (!await users.ExistsAsync(id))
                ex.AddError("User", ValidationErrorCode.UserDoesNotExist);

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

        private async Task<ValidationException> ValidateAssociation(SystemGroupPermissionAssociationID id)
        {
            var ex = new ValidationException();

            if (await groupPermissions.ExistsAsync(id))
                ex.AddError(nameof(SystemGroupPermissionAssociation), ValidationErrorCode.GroupPermissionAssociationAlreadyExists);

            return ex;
        }
    }
}
