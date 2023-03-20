using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Unit>
    {
        private readonly ICourseRepository courses;

        public UpdateCourseCommandHandler(ICourseRepository courses)
        {
            this.courses = courses;
        }

        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var idCourse = new CourseID(request.IdCourse);

            var course = await courses.GetAsync(idCourse);

            course.Update(request.Name, request.Description, request.StartDate, request.EndDate);

            return Unit.Value;
        }
    }
}
