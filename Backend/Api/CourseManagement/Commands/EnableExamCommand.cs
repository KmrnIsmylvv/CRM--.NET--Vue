using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class EnableExamCommand : ICommand
    {
        public int IdExam { get; set; }
    }
}
