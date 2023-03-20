using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Unit>
    {
        private readonly IExamRepository exams;
        private readonly ExamValidationService examValidationService;

        public UpdateExamCommandHandler(IExamRepository exams, ExamValidationService examValidationService)
        {
            this.exams = exams;
            this.examValidationService = examValidationService;
        }

        public async Task<Unit> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var ex = await examValidationService.ValidateExamNameIsUnique(request.ExamName);

            var exam = await exams.GetAsync(new ExamID(request.IdExam));

            ex.CollectErrors(() =>
            {
                exam.Update(request.ExamName, request.ExamDate, request.StartTime, request.EndTime);
            }).TryThrow();

            return Unit.Value;
        }
    }
}
