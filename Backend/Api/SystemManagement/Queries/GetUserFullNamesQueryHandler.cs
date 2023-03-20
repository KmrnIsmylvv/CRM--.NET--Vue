using Elfo.Round.ReadCycle;
using Elfo.Contoso.LearningRoundKamran.Api.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetUserFullNamesQueryHandler : IRequestHandler<GetUserFullNames.Query, List<GetUserFullNames.Result>>
    {
        readonly IDbConnection connection;
        public GetUserFullNamesQueryHandler(IDbConnection connection) => this.connection = connection;

        public Task<List<GetUserFullNames.Result>> Handle(GetUserFullNames.Query query, CancellationToken cancellationToken) =>
            connection.ReadAsync(q => q
                .Where("isEnabled", RuleOperator.IsEqual, true)
                .ToMultiSelectionResultAsync<GetUserFullNames.Result>(query, "idUser", "fullName"));
    }
}
