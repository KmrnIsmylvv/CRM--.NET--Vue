using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement
{
    public class CourseValidationService
    {
        readonly ICourseRepository courses;
        readonly ISystemUserRepository users;

        public CourseValidationService(ICourseRepository courses, ISystemUserRepository users)
        {
            this.courses = courses;
            this.users = users;
        }

        public async Task<ValidationException> ValidateCourseNameIsUnique(string courseName, CourseID idCourse = null)
        {
            var ex = new ValidationException();

            if (await courses.ExistsWithName(courseName, idCourse))
                ex.AddError(nameof(Course.Name), ValidationErrorCode.CourseNameAlreadyExists);

            return ex;
        }

        public async Task<ValidationException> ValidateTeacher(SystemUserID idUser)
        {
            var ex = new ValidationException();

            if (!await users.IsTeacher(idUser))
                ex.AddError(nameof(Course.IdTeacher), ValidationErrorCode.InvalidTeacher);

            return ex;
        }
    }
}
