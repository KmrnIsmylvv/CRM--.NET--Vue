using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects
{
    public class LessonID : ID<int>
    {
        public LessonID()
        {
            Value = default;
        }

        public LessonID(int id)
        {
            Value = id;
        }
    }
}
