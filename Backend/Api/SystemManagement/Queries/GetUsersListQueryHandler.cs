using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersList.Query, ListResult<GetUsersList.Result>>
    {
        readonly IDbConnection connection;
        public GetUsersListQueryHandler(IDbConnection connection) => this.connection = connection;

        public Task<ListResult<GetUsersList.Result>> Handle(GetUsersList.Query query, CancellationToken cancellationToken) =>
            connection.ReadAsync(q => q
                .Where(query.Filter)
                .Page(query.Paging)
                .OrderBy(query.Sorting?.Fields.ToArray())
                .ToListResultAsync<GetUsersList.Result>());
    }
}
