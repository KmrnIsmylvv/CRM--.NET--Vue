using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class EnableCourseCommand : ICommand
    {
        public int IdCourse { get; set; }
    }
}
