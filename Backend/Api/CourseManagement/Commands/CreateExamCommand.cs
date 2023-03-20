using System;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateExamCommand : ICommand<int>
    {
        public string ExamName { get; set; }
        public DateTimeOffset ExamDate { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        public int IdCourse { get; set; }


    }
}
