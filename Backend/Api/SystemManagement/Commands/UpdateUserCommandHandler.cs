using Elfo.Round;
using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        readonly ISystemUserRepository users;
        readonly ISystemGroupRepository groups;
        readonly ISystemUserGroupAssociationRepository userGroups;
        readonly ISystemUserPermissionAssociationRepository userPermissions;

        public UpdateUserCommandHandler(ISystemUserRepository users, ISystemGroupRepository groups,
                                        ISystemUserGroupAssociationRepository userGroups,
                                        ISystemUserPermissionAssociationRepository userPermissions)
        {
            this.users = users;
            this.groups = groups;
            this.userGroups = userGroups;
            this.userPermissions = userPermissions;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await users.GetAsync(new SystemUserID(command.IdUser))
                ?? throw new ValidationException(nameof(SystemUser), ValidationErrorCode.UserDoesNotExist);

            var ex = new ValidationException();

            if (!string.Equals(user.Username, command.Username, StringComparison.InvariantCultureIgnoreCase))
                ex += await ValidateUsername(command.Username);

            ex.CollectErrors(() =>
                user.Update(command.Username, command.LanguageCode, command.CultureCode,
                    command.FirstName, command.LastName, command.Email, command.TelephoneNumber))
                .TryThrow();

            var userAssociations = await userGroups.GetByIdUser(user.Id);

            var (groupsToRemove, groupsToAdd) = userAssociations.Select(a => a.Id.IdGroup).ToList()
                .Differences(command.IdGroupList.Select(g => new SystemGroupID(g)).ToList());

            foreach (var a in (await Associate(user.Id, groupsToAdd)))
                await userGroups.AddAsync(a);

            foreach (var a in groupsToRemove.Select(g => new SystemUserGroupAssociationID(user.Id, g)).ToList())
                await userGroups.RemoveAsync(a);

            var permissionAssociations = await userPermissions.GetByIdUser(user.Id);

            var (permissionsToRemove, permissionsToAdd) = permissionAssociations.Select(a => a.Id.Permission).ToList()
                .Differences(command.Permissions.Select(p => p).ToList());

            foreach (var a in (await Associate(user.Id, permissionsToAdd)))
                await userPermissions.AddAsync(a);

            foreach (var a in permissionsToRemove.Select(p => new SystemUserPermissionAssociationID(user.Id, p)).ToList())
                await userPermissions.RemoveAsync(a);

            return Unit.Value;
        }

        private async Task<ValidationException> ValidateUsername(string username)
        {
            var ex = new ValidationException();

            if (await users.ExistsWithUsername(username))
                ex.AddError(nameof(SystemUser.Username), ValidationErrorCode.UsernameAlreadyExists);

            return ex;
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

        private async Task<List<SystemUserPermissionAssociation>> Associate(SystemUserID idUser, IEnumerable<string> permissions)
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

        private async Task<ValidationException> ValidateIdGroup(SystemGroupID id)
        {
            var ex = new ValidationException();

            if (!await groups.ExistsAsync(id))
                ex.AddError("Group", ValidationErrorCode.GroupDoesNotExist);

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

        private async Task<ValidationException> ValidateAssociation(SystemUserPermissionAssociationID id)
        {
            var ex = new ValidationException();

            if (await userPermissions.ExistsAsync(id))
                ex.AddError(nameof(SystemUserPermissionAssociation), ValidationErrorCode.UserPermissionAssociationAlreadyExists);

            return ex;
        }
    }
}
