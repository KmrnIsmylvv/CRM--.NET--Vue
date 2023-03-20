using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class EnableCourseCommandHandler : IRequestHandler<EnableCourseCommand, Unit>
    {
        private readonly ICourseRepository courses;

        public EnableCourseCommandHandler(ICourseRepository courses) => this.courses = courses;

        public async Task<Unit> Handle(EnableCourseCommand request, CancellationToken cancellationToken)
        {
            CourseID idCourse = new (request.IdCourse);

            var course = await courses.GetAsync(idCourse);

            course.Enable();

            return Unit.Value;
        }
    }
}
