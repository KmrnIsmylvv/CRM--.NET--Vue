using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, int>
    {
        private readonly IExamRepository exams;
        private readonly ExamValidationService examValidationService;

        public CreateExamCommandHandler(IExamRepository exams, ExamValidationService examValidationService)
        {
            this.exams = exams;
            this.examValidationService = examValidationService;
        }
        public async Task<int> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var ex = await examValidationService.ValidateExamNameIsUnique(request.ExamName) +
                await examValidationService.ValidateCourse(new CourseID(request.IdCourse));

            Exam exam = null;

            ex.CollectErrors(() =>
            {
                exam = Exam.Create(new CourseID(request.IdCourse), request.ExamName, request.ExamDate,
                    request.StartTime, request.EndTime);
            }).TryThrow();

            await exams.AddAsync(exam);

            return exam.Id.Value;
        }
    }
}
