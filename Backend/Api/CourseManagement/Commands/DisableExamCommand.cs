using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class DisableExamCommand : ICommand
    {
        public int IdExam { get; set; }
    }
}
