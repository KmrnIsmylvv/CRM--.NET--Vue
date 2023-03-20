using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateLessonCommand : ICommand
    {
        public string Name{ get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }   
        public int IdCourse { get; set; }
    }
}
