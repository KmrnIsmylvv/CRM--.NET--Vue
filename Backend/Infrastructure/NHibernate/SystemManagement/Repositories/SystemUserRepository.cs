using Elfo.Round.WriteCycle;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.SystemManagement
{
    public class SystemUserRepository : BaseRepository<SystemUser, SystemUserID>, ISystemUserRepository
    {
        public SystemUserRepository(ISession session) : base(session) { }

        public async Task<bool> ExistsWithUsername(string username) =>
            await session.Query<SystemUser>()
                .CountAsync(u => u.Username == username && u.IsEnabled) > 0;

        public override async Task<bool> ExistsAsync(SystemUserID id) =>
            await session.Query<SystemUser>().CountAsync(u => u.Id == id && u.IsEnabled) > 0;

        public async Task<bool> IsTeacher(SystemUserID idUser)
        {
            #region GetByCode
            // Retrieve user based on Id
            //var user = await session.Query<SystemUser>().SingleOrDefaultAsync(x => x.Id == idUser);
            //if (user is null) return false;

            //// Retrieve group that is teacherGroup
            //var teacherGroup = await session.Query<SystemGroup>().SingleAsync(x => x.Name.ToLower() == "teacher");

            //// See if the user inside in this group
            //var association = await session.Query<SystemUserGroupAssociation>()
            //    .SingleOrDefaultAsync(x => x.Id.IdUser == user.Id && x.Id.IdGroup == teacherGroup.Id);

            //return association != null;
            #endregion

            #region GetByQueries
            //IQuery query = session.GetNamedQuery("IsTeacher");

            //query.SetInt32("idUser", idUser.Value);

            //int result = query.UniqueResult<int>();

            //return result != default;
            #endregion

            #region Use Stored Procedure
            var result = await session.CreateSQLQuery("exec checkIsTeacher_sp :idUser")
                 .SetInt32("idUser", idUser.Value)
                 .UniqueResultAsync<bool>();

            return result;
            #endregion
        }
    }
}
