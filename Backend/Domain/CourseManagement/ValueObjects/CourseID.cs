using System.Collections.Generic;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects
{
    public class CourseID : ID<int>
    {
        public CourseID()
        {
            Value = default;
        }

        public CourseID(int id)
        {
            Value = id;
        }
    }
}
