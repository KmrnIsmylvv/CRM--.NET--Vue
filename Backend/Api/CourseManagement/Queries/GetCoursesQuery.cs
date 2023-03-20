using System;
using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCourses
    {
        public class Query : ListModel, IRequest<ListResult<Result>> { }


        [DataSource("courses_vw")]
        public class Result
        {
            [Column] public int IdCourse { get; set; }
            [Column] public string Name { get; set; }
            [Column] public string Description { get; set; }
            [Column] public string TeacherFullName { get; set; }
            [Column] public DateTimeOffset StartDate { get; set; }
            [Column] public DateTimeOffset EndDate { get; set; }
            [Column] public bool IsEnabled { get; set; }

            [Column] public int IdTeacher { get; set; }
        }
    }
}
