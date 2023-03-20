using Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.Test;
using System.Threading;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Api.Test
{
    [TestClass]
    public class UpdateUserTest
    {
        ISystemUserRepository users;
        ISystemGroupRepository groups;
        ISystemUserGroupAssociationRepository userGroups;
        ISystemUserPermissionAssociationRepository userPermissions;

        UpdateUserCommandHandler handler;

        [TestInitialize]
        public void Initialize()
        {
            users = Substitute.For<ISystemUserRepository>();
            groups = Substitute.For<ISystemGroupRepository>();
            userGroups = Substitute.For<ISystemUserGroupAssociationRepository>();
            userPermissions = Substitute.For<ISystemUserPermissionAssociationRepository>();

            handler = new UpdateUserCommandHandler(users, groups, userGroups, userPermissions);
        }

        [TestMethod]
        public async Task Default_Ok()
        {
            var user = new SystemUserBuilder().Build();
            var command = new UpdateUserCommand
            {
                IdUser = 1,
                Username = $"elfo-2k\name.surname",
                FirstName = "Name",
                LastName = "Surname",
                Email = "name.surname@elfo.net",
                LanguageCode = "en",
                CultureCode = "it-IT",
                TelephoneNumber = "+0523000000",
                IdGroupList = new List<short>(),
                Permissions = new List<string>()
            };

            users.GetAsync(new SystemUserID(command.IdUser)).Returns(user);
            users.ExistsWithUsername(command.Username).Returns(false);
            userGroups.GetByIdUser(user.Id).Returns(new List<SystemUserGroupAssociation>());
            userPermissions.GetByIdUser(user.Id).Returns(new List<SystemUserPermissionAssociation>());

            await handler.Handle(command, CancellationToken.None);
        }
    }
}
