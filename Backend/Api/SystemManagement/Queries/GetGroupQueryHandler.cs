using Dapper;
using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetGroupQueryHandler : IRequestHandler<GetGroup.Query, GetGroup.Result>
	{
		readonly IDbConnection connection;
		public GetGroupQueryHandler(IDbConnection connection) => this.connection = connection;

		public async Task<GetGroup.Result> Handle(GetGroup.Query query, CancellationToken cancellationToken)
        {
			GetGroup.Result result;
			connection.Open();

			using (var trans = connection.BeginTransaction())
			{
				result = await connection.QueryBuilder(trans)
					.Where("idGroup", RuleOperator.IsEqual, query.IdGroup)
					.SingleAsync<GetGroup.Result>();

				result.IdUserList = await connection.QueryAsync<int>(
					$@"select [idUser] from [round].[usersGroups_tb] where idGroup = @{ nameof(query.IdGroup) }",
					new { query.IdGroup }, trans);

				result.Permissions = await connection.QueryAsync<string>(
					$@"select [permission] from [round].[groupsPermissions_tb] where idGroup = @{ nameof(query.IdGroup) }",
					new { query.IdGroup }, trans);

				trans.Commit();
			}

			return result;
		}
    }
}
