using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class DisableCourseCommand : ICommand
    {
        public int IdCourse { get; set; }
    }
}
