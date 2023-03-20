using System;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class UpdateExamCommand : ICommand
    {
        public int IdExam { get; set; }
        public string ExamName { get; set; }
        public DateTimeOffset ExamDate { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
