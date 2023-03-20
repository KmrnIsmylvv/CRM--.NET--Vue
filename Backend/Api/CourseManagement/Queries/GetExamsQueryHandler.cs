using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement;
using Microsoft.AspNetCore.Http;
using Elfo.Contoso.LearningRoundKamran.Domain.SystemManagement.ValueObjects;
using System.Linq;
using Elfo.Round.Identity;

namespace Elfo.Contoso.LearningRoundKamran.Api.CourseManagement.Queries
{
    public class GetExamsQueryHandler : IRequestHandler<GetExams.Query, ListResult<GetExams.Result>>
    {
        readonly IDbConnection connection;

        public GetExamsQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }
        public async Task<ListResult<GetExams.Result>> Handle(GetExams.Query query, CancellationToken cancellationToken)
        {
            return await connection.ReadAsync(q => q
                .Where(query.Filter)
                .Page(query.Paging)
                .OrderBy(query.Sorting?.Fields.ToArray())
                .ToListResultAsync<GetExams.Result>());
        }
    }
}
