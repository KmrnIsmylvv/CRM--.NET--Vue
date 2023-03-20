using System.Threading;
using System.Threading.Tasks;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.Repositories;
using Elfo.Contoso.LearningRoundKamran.Domain.CourseManagement.ValueObjects;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Commands
{
    public class EnableExamCommandHandler : IRequestHandler<EnableExamCommand, Unit>
    {
        private readonly IExamRepository exams;

        public EnableExamCommandHandler(IExamRepository exams) => this.exams = exams;

        public async Task<Unit> Handle(EnableExamCommand request, CancellationToken cancellationToken)
        {
            var exam =await exams.GetAsync(new ExamID(request.IdExam));

            exam.Enable();

            return Unit.Value;
        }
    }
}
