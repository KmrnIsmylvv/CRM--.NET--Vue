using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetCourseQueryHandler : IRequestHandler<GetCourse.Query, GetCourse.Result>
    {
        private readonly IDbConnection connection;

        public GetCourseQueryHandler(IDbConnection connection) => this.connection = connection;

        public async Task<GetCourse.Result> Handle(GetCourse.Query query, CancellationToken cancellationToken)
        {
            GetCourse.Result result;
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                result = await connection.QueryBuilder(trans)
                    .Where("idCourse", RuleOperator.IsEqual, query.IdCourse)
                    .SingleAsync<GetCourse.Result>();

                trans.Commit();
            }

            return result;

        }
    }
}