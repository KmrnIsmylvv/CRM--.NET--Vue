using System.Threading.Tasks;
using Elfo.Round.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries;
using Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands;
using System;
using Elfo.Round.Identity;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Controllers
{
    [Route("api/courses")]
    [DBAuthorize(Permissions.CanReadCourse)]
    public class CourseController : ControllerBase
    {
        readonly IMediator mediator;

        public CourseController(IMediator mediator) => this.mediator = mediator;

        #region CourseQueries
        [HttpGet("{idCourse}")]
        public async Task<ActionResult<GetCourse.Result>> GetCourse([FromRoute] int idCourse)
        {
            var result = await mediator.Send(new GetCourse.Query { IdCourse = idCourse });
            return this.ResultOk(result);
        }

        [HttpPost()]
        public async Task<ActionResult<ListResult<GetCourses.Result>>> GetCourses([FromBody] GetCourses.Query query)
        {
            var result = await mediator.Send(query);
            return this.ResultOk(result);
        }

        [HttpPost("getCoursesNames")]
        public async Task<ActionResult<ListResult<GetCoursesNames.Result>>> GetCoursesNames([FromBody] GetCoursesNames.Query query)
        {
            var result = await mediator.Send(query);
            return this.ResultOk(result);
        }

        [HttpGet("{idCourse}/lessons/{idLesson}")]
        public async Task<ActionResult<GetLesson.Result>> GetLesson([FromRoute] int idCourse,
            [FromRoute] int idLesson)
        {
            var result = await mediator.Send(new GetLesson.Query { IdCourse = idCourse, IdLesson = idLesson });
            return this.ResultOk(result);
        }

        [HttpPost("{idCourse}/lessons")]
        public async Task<ActionResult<ListResult<GetLessons.Result>>> GetLessons([FromRoute] int idCourse)
        {
            var result = await mediator.Send(new GetLessons.Query { IdCourse = idCourse });
            return this.ResultOk(result);
        }
        #endregion

        #region CourseCommands

        [HttpPost("create")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult<int>> CreateCourse([FromBody] CreateCourseCommand command)
        {
            var result = await mediator.Send(command);
            return this.ResultOk(result);
        }

        [HttpPost("{idCourse}/update")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> UpdateCourse([FromRoute] int idCourse,
            [FromBody] UpdateCourseDto courseDto)
        {
            var command = new UpdateCourseCommand
            {
                IdCourse = idCourse,
                Name = courseDto.Name,
                Description = courseDto.Description,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate,
            };

            await mediator.Send(command);
            return this.ResultOk();
        }

        [HttpPost("{idCourse}/enable")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> EnableCourse([FromRoute] int idCourse)
        {
            await mediator.Send(new EnableCourseCommand { IdCourse = idCourse });
            return this.ResultOk();
        }

        [HttpPost("{idCourse}/disable")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> DisableCourse([FromRoute] int idCourse)
        {
            await mediator.Send(new DisableCourseCommand { IdCourse = idCourse });
            return this.ResultOk();
        }

        [HttpPost("{idCourse}/lessons/add")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> AddLesson([FromRoute] int idCourse,
            [FromBody] AddLessonDto addLessonDto)
        {
            var command = new CreateLessonCommand
            {
                IdCourse = idCourse,
                Name = addLessonDto.Name,
                Description = addLessonDto.Description,
                Duration = addLessonDto.Duration,
            };

            await mediator.Send(command);
            return this.ResultOk();
        }

        [HttpPost("{idCourse}/lessons/{idLesson}/update")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> UpdateLesson([FromRoute] int idCourse, [FromRoute] int idLesson,
            [FromBody] UpdateLessonDto updateLessonDto)
        {
            var command = new UpdateLessonCommand
            {
                IdCourse = idCourse,
                IdLesson = idLesson,
                Name = updateLessonDto.Name,
                Description = updateLessonDto.Description,
                Duration = updateLessonDto.Duration,
            };

            await mediator.Send(command);
            return this.ResultOk();
        }

        [HttpDelete("{idCourse}/lessons/{idLesson}")]
        [DBAuthorize(Permissions.CanWriteCourse)]
        public async Task<ActionResult> RemoveLesson([FromRoute] int idCourse, [FromRoute] int idLesson)
        {
            await mediator.Send(new RemoveLessonCommand { IdCourse=idCourse, IdLesson = idLesson});
            return this.ResultOk();
        }
        #endregion

        #region Dtos
        public class UpdateCourseDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public DateTimeOffset EndDate { get; set; }
        }
        public class AddLessonDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
        }

        public class UpdateLessonDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
        }

        #endregion
    }

}
