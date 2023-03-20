using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
	public class GetGroupsList
	{
        public class Query : ListModel, IRequest<ListResult<Result>> { }

		[DataSource("GetGroups", DataSourceType.FileQuery)]
		public class Result
		{
			[Column] public string IdGroup { get; set; }
			[Column] public string Name { get; set; }
			[Column] public bool IsEnabled { get; set; }
			[Column] public string UserFullNames { get; set; }
			[Column] public string Permissions { get; set; }
		}
	}
}
