using Dapper;
using Elfo.Round.ReadCycle;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
	public class GetUserQueryHandler : IRequestHandler<GetUser.Query, GetUser.Result>
    {
		readonly IDbConnection connection;
		public GetUserQueryHandler(IDbConnection connection) => this.connection = connection;

		public async Task<GetUser.Result> Handle(GetUser.Query query, CancellationToken cancellationToken)
        {
			GetUser.Result result;
			connection.Open();

			using (var trans = connection.BeginTransaction())
			{
				result = await connection.QueryBuilder(trans)
					.Where("idUser", RuleOperator.IsEqual, query.IdUser)
					.SingleAsync<GetUser.Result>();

				result.IdGroupList = await connection.QueryAsync<short>(
					$@"select [idGroup] from [round].[usersGroups_tb] where idUser = @{ nameof(query.IdUser) }",
					new { query.IdUser }, trans);

				result.Permissions = await connection.QueryAsync<string>(
					$@"select [permission] from [round].[usersPermissions_tb] where idUser = @{ nameof(query.IdUser) }",
					new { query.IdUser }, trans);

				trans.Commit();
			}

			return result;
		}
    }
}
