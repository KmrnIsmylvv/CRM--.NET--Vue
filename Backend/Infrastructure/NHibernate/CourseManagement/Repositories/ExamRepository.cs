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
    public class ExamRepository : BaseRepository<Exam, ExamID>, IExamRepository
    {
        public ExamRepository(ISession session) : base(session) { }

        public async Task<bool> ExistsWithName(string examName, ExamID idExam = null)
        {
            var query = session.Query<Exam>();

            if (string.IsNullOrWhiteSpace(examName))
                return false;

            if (idExam is not null)
                query = query.Where(x => x.Id != idExam);

            return await query.Where(x => x.ExamName.ToLower() == examName.ToLower()).CountAsync() > 0;
        }
    }
}
