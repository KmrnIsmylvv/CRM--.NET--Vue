using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories
{
    public interface ICourseRepository : IRepository<Course, CourseID>
    {
        Task<bool> ExistsWithName(string courseName, CourseID idCourse);
    }
}
