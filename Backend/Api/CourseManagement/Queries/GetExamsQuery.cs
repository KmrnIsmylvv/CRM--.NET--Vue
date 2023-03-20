using System;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetExams
    {
        public class Query : ListModel, IRequest<ListResult<Result>> { }

        [DataSource("exams_vw")]
        public class Result
        {
            [Column] public int IdExam { get; set; }
            [Column] public string ExamName { get; set; }
            [Column] public DateTimeOffset ExamDate { get; set; }
            [Column] public DateTimeOffset StartTime { get; set; }
            [Column] public DateTimeOffset EndTime { get; set; }
            [Column] public bool IsEnabled { get; set; }
            [Column] public string CourseName { get; set; }

            [Column] public int IdCourse { get; set; }
        }
    }
}
