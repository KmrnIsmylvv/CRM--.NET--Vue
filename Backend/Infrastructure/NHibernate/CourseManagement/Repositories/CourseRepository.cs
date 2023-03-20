using System.Linq;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;
using NHibernate;
using NHibernate.Linq;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.CourseManagement.Repositories
{
    public class CourseRepository : BaseRepository<Course, CourseID>, ICourseRepository
    {
        public CourseRepository(ISession session) : base(session) { }

        public async Task<bool> ExistsWithName(string courseName, CourseID idCourse = null)
        {
            var query = session.Query<Course>();

            if (string.IsNullOrWhiteSpace(courseName))
                return false;

            if (idCourse is not null)
                query = query.Where(x => x.Id != idCourse);

            return await query.Where(x => x.Name.ToLower() == courseName.ToLower()).CountAsync() > 0;
        }
    }
}
