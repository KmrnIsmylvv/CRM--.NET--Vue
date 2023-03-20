using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Unit>
    {
        private readonly ICourseRepository courses;

        public CreateLessonCommandHandler(ICourseRepository courses) => this.courses = courses;

        public async Task<Unit> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            CourseID idCourse = new(request.IdCourse);

            var course = await courses.GetAsync(idCourse);

            course.AddLesson(request.Name, request.Description, request.Duration);

            return Unit.Value;
        }
    }
}
