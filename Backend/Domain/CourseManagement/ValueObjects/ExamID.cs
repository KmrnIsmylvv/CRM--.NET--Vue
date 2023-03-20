using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects
{
    public class ExamID : ID<int>
    {
        public ExamID()
        {
            Value = default;
        }

        public ExamID(int id)
        {
            Value = id;
        }
    }
}
