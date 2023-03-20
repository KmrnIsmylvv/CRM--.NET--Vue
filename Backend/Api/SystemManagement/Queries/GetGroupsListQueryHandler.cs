using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetGroupsListQueryHandler : IRequestHandler<GetGroupsList.Query, ListResult<GetGroupsList.Result>>
    {
        readonly IDbConnection connection;
        public GetGroupsListQueryHandler(IDbConnection connection) => this.connection = connection;

        public Task<ListResult<GetGroupsList.Result>> Handle(GetGroupsList.Query query, CancellationToken cancellationToken) =>
            connection.ReadAsync(q => q
                .Where(query.Filter)
                .Page(query.Paging)
                .OrderBy(query.Sorting?.Fields.ToArray())
                .ToListResultAsync<GetGroupsList.Result>());
    }
}
