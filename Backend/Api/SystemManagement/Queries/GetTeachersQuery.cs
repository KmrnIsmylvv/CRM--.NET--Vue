using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2013.Excel;
using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetTeachers
    {
        public class Query : SelectionModel, IRequest<List<Result>>
        {

        }

        [DataSource("teachers_vw")]
        public class Result
        {
            [Column] public int IdTeacher { get; set; }
            [Column] public string TeacherName { get; set; }
        }
    }
}
