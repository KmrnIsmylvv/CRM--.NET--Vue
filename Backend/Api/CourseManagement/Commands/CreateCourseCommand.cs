using System;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateCourseCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public int IdTeacher { get; set; }
    }
}
