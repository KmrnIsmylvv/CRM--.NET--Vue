using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using MediatR;
using NHibernate.Cfg.MappingSchema;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateCourseCommmandHandler : IRequestHandler<CreateCourseCommand, int>
    {
        readonly ICourseRepository courses;
        readonly CourseValidationService courseValidationService;

        public CreateCourseCommmandHandler(ICourseRepository courses, CourseValidationService courseValidationService)
        {
            this.courses = courses;
            this.courseValidationService = courseValidationService;
        }

        public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var ex = await courseValidationService.ValidateCourseNameIsUnique(request.Name) +
                await courseValidationService.ValidateTeacher(new SystemUserID(request.IdTeacher));

            Course course = null;
            ex.CollectErrors(() =>
            {
                course = Course.Create(request.Name, request.Description, new(request.IdTeacher), request.StartDate, request.EndDate);
            }).TryThrow();

            await courses.AddAsync(course);

            return course.Id.Value;
        }
    }
}
