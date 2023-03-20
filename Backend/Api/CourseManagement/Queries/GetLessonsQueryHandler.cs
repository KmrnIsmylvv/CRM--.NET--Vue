using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetLessonsQueryHandler : IRequestHandler<GetLessons.Query, ListResult<GetLessons.Result>>
    {
        readonly IDbConnection connection;
        public GetLessonsQueryHandler(IDbConnection connection) => this.connection = connection;

        public Task<ListResult<GetLessons.Result>> Handle(GetLessons.Query query, CancellationToken cancellationToken) =>
            connection.ReadAsync(q => q
                .Where(query.Filter)
                .Where("idCourse", RuleOperator.IsEqual, query.IdCourse)
                .Page(query.Paging)
                .OrderBy(query.Sorting?.Fields.ToArray())
                .ToListResultAsync<GetLessons.Result>());
    }
}
