using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class DisableExamCommandHandler : IRequestHandler<DisableExamCommand, Unit>
    {
        private readonly IExamRepository exams;

        public DisableExamCommandHandler(IExamRepository exams) => this.exams = exams;
        public async Task<Unit> Handle(DisableExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await exams.GetAsync(new ExamID(request.IdExam));

            exam.Disable();

            return Unit.Value;

        }
    }
}
