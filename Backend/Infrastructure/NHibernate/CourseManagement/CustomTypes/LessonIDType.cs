using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.CourseManagement
{
    public class LessonIDType : CustomUserType<LessonID, int>
    {
    }
}
