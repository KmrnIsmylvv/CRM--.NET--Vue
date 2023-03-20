using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetLessonQueryHandler : IRequestHandler<GetLesson.Query, GetLesson.Result>
    {
        private readonly IDbConnection connection;

        public GetLessonQueryHandler(IDbConnection connection) => this.connection = connection;

        public async Task<GetLesson.Result> Handle(GetLesson.Query query, CancellationToken cancellationToken)
        {
            GetLesson.Result result;
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                result = await connection.QueryBuilder(trans)
                    .Where("idLesson", RuleOperator.IsEqual, query.IdLesson)
                    .SingleAsync<GetLesson.Result>();

                trans.Commit();
            }

            return result;
        }
    }
}
