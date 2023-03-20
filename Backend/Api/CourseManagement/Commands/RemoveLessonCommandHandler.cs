using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class RemoveLessonCommandHandler : IRequestHandler<RemoveLessonCommand, Unit>
    {
        private readonly ICourseRepository courses;

        public RemoveLessonCommandHandler(ICourseRepository courses) => this.courses = courses;


        public async Task<Unit> Handle(RemoveLessonCommand request, CancellationToken cancellationToken)
        {
            CourseID idCourse = new(request.IdCourse);

            var course = await courses.GetAsync(idCourse);

            course.RemoveLesson(new LessonID(request.IdLesson));

            return Unit.Value;
        }
    }
}
