using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class DisableCourseCommandHandler : IRequestHandler<DisableCourseCommand, Unit>
    {
        private readonly ICourseRepository courses;

        public DisableCourseCommandHandler(ICourseRepository courses) => this.courses = courses;

        public async  Task<Unit> Handle(DisableCourseCommand request, CancellationToken cancellationToken)
        {
            CourseID idCourse = new(request.IdCourse);

            var course = await courses.GetAsync(idCourse);

            course.Disable();

            return Unit.Value;
        }
    }
}
