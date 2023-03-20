using Elfo.Round.ReadCycle;
using MediatR;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCoursesNames
    {
        public class Query : SelectionModel, IRequest<List<Result>> { }


        [DataSource("coursesName_vw")]
        public class Result
        {
            [Column] public int IdCourse { get; set; }
            [Column] public string IdTeacher { get; set; }
            [Column] public string CourseName { get; set; }
        }
    }
}
