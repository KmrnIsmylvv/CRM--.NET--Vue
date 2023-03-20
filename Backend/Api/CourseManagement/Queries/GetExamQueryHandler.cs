using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetExamQueryHandler : IRequestHandler<GetExam.Query, GetExam.Result>
    {
        private readonly IDbConnection connection;

        public GetExamQueryHandler(IDbConnection connection) => this.connection = connection;

        public async Task<GetExam.Result> Handle(GetExam.Query query, CancellationToken cancellationToken)
        {
            GetExam.Result result;
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                result = await connection.QueryBuilder(trans)
                    .Where("idExam", RuleOperator.IsEqual, query.IdExam)
                    .SingleAsync<GetExam.Result>();

                trans.Commit();
            }

            return result;

        }
    }
}
