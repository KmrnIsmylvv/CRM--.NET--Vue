using System;
using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class UpdateCourseCommand : ICommand
    {
        public int IdCourse { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
