using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories
{
    public interface IExamRepository :IRepository<Exam, ExamID>
    {
        Task<bool> ExistsWithName(string examName, ExamID idExam);
    }
}
