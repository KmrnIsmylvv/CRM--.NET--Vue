using Elfo.Round.ReadCycle;
using Hangfire.Annotations;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetLessons
    {
        public class Query : ListModel, IRequest<ListResult<Result>> 
        {
            public int IdCourse { get; set; }
        }

        [DataSource("lessons_vw")]
        public class Result
        {
            [Column] public int IdLesson { get; set; }
            [Column] public int IdCourse { get; set; }
            [Column] public string Name { get; set; }
            [Column] public string Description { get; set; }
            [Column] public int Duration { get; set; }
            [Column] public string CourseName { get; set; }
        }


        
    }
}
