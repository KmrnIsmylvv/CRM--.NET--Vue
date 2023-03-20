using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetLesson
    {
        public class Query : IRequest<Result>
        {
            public int IdLesson { get; set; }
            public int IdCourse { get; set; }

        }

        [DataSource("lessons_vw")]
        public class Result
        {
            [Column] public int IdCourse { get; set; }
            [Column] public string Name { get; set; }
            [Column] public string Description { get; set; }
            [Column] public int Duration { get; set; }
            [Column]public string CourseName { get; set; }
        }
    }
}
