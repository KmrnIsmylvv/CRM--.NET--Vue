using Elfo.Round.ReadCycle;
using MediatR;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
	public class GetUser
    {
        public class Query : IRequest<Result>
		{
			public int IdUser { get; set; }
		}

		[DataSource("GetUsers", DataSourceType.FileQuery)]
		public class Result
		{
			[Column] public int IdUser { get; set; }
			[Column] public string Username { get; set; }
			[Column] public string FirstName { get; set; }
			[Column] public string LastName { get; set; }
			[Column] public string FullName { get; set; }
			[Column] public string Email { get; set; }
			[Column] public string LanguageCode { get; set; }
			[Column] public string CultureCode { get; set; }
			[Column] public string TelephoneNumber { get; set; }
			public IEnumerable<short> IdGroupList { get; set; }
			public IEnumerable<string> Permissions { get; set; }
		}
	}
}
