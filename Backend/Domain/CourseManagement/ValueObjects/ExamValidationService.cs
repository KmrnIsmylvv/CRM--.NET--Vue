using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;

namespace Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects
{
    public class ExamValidationService
    {
        private readonly IExamRepository exams;
        private readonly ICourseRepository courses;

        public ExamValidationService(IExamRepository exams, ICourseRepository courses)
        {
            this.exams = exams;
            this.courses = courses;
        }

        public async Task<ValidationException> ValidateExamNameIsUnique(string examName, ExamID idExam = null)
        {
            var ex = new ValidationException();

            if (await exams.ExistsWithName(examName, idExam))
                ex.AddError(nameof(Exam.ExamName), ValidationErrorCode.ExamNameAlreadyExists);

            return ex;
        }

        public async Task<ValidationException> ValidateCourse(CourseID idCourse)
        {
            var ex = new ValidationException();

            if (!await courses.ExistsAsync(idCourse))
                ex.AddError(nameof(idCourse), ValidationErrorCode.InvalidCourse);

            return ex;
        }
    }
}
