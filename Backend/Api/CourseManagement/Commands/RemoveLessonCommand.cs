using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class RemoveLessonCommand : ICommand
    {
        public int IdLesson { get; set; }
        public int IdCourse { get; set; }
    }
}
