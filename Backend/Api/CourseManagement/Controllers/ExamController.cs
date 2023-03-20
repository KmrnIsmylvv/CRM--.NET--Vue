using Antlr.Runtime.Tree;
using Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands;
using Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;
using Elfo.Round.Identity;
using Elfo.Round.Mvc;
using Elfo.Round.ReadCycle;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.ExamManagement.Controllers
{
    [Route("api/exams")]
    [DBAuthorize(Permissions.CanReadExam)]
    public class ExamController : ControllerBase
    {
        readonly IMediator mediator;

        public ExamController(IMediator mediator) => this.mediator = mediator;

        #region ExamQueries
        [HttpGet("{idExam}")]
        public async Task<ActionResult<GetExam.Result>> GetExam([FromRoute] int idExam)
        {
            var result = await mediator.Send(new GetExam.Query { IdExam = idExam });
            return this.ResultOk(result);
        }

        [HttpPost()]
        public async Task<ActionResult<ListResult<GetExams.Result>>> GetExams([FromBody] GetExams.Query query)
        {
            var result = await mediator.Send(query);
            return this.ResultOk(result);
        }
        #endregion

        #region ExamCommands

        [HttpPost("create")]
        [DBAuthorize(Permissions.CanWriteExam)]
        public async Task<ActionResult<int>> CreateExam([FromBody] CreateExamCommand command)
        {
            var result = await mediator.Send(command);
            return this.ResultOk(result);
        }

        [HttpPost("{idExam}/update")]
        [DBAuthorize(Permissions.CanWriteExam)]
        public async Task<ActionResult> UpdateExam([FromRoute] int idExam,
            [FromBody] UpdateExamDto examDto)
        {
            var command = new UpdateExamCommand
            {
                IdExam = idExam,
                ExamName = examDto.ExamName,
                ExamDate = examDto.ExamDate,
                StartTime = examDto.StartTime,
                EndTime = examDto.EndTime,
            };

            await mediator.Send(command);
            return this.ResultOk();
        }

        [HttpPost("{idExam}/enable")]
        [DBAuthorize(Permissions.CanWriteExam)]
        public async Task<ActionResult> EnableExam([FromRoute] int idExam)
        {
            await mediator.Send(new EnableExamCommand { IdExam = idExam });
            return this.ResultOk();
        }

        [HttpPost("{idExam}/disable")]
        [DBAuthorize(Permissions.CanWriteExam)]
        public async Task<ActionResult> DisableExam([FromRoute] int idExam)
        {
            await mediator.Send(new DisableExamCommand { IdExam = idExam });
            return this.ResultOk();
        }

        #endregion

        #region Dtos
        public class UpdateExamDto
        {
            public string ExamName { get; set; }
            public DateTimeOffset ExamDate { get; set; }
            public DateTimeOffset StartTime { get; set; }
            public DateTimeOffset EndTime { get; set; }
        }
        #endregion
    }
}
