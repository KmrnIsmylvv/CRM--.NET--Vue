using System;
using Elfo.Round.ReadCycle;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCourse
    {

        public class Query : IRequest<Result>
        {
            public int IdCourse { get; set; }
        }


        [DataSource("dbo.courses_vw")]
        public class Result
        {
            [Column] public int IdCourse { get; set; }
            [Column] public string Name { get; set; }
            [Column] public string Description { get; set; }
            [Column] public string TeacherFullname { get; set; }
            [Column] public int IdTeacher { get; set; }
            [Column] public DateTimeOffset StartDate { get; set; }
            [Column] public DateTimeOffset EndDate { get; set; }
            [Column] public bool IsEnabled { get; set; }
        }
    }
}