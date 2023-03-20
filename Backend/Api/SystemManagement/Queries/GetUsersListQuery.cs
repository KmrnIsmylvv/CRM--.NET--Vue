using Elfo.Round.ReadCycle;
using MediatR;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Queries
{
    public class GetUsersList
    {
        public class Query : ListModel, IRequest<ListResult<Result>> { }

        [DataSource("GetUsers", DataSourceType.FileQuery)]
        public class Result
        {
            [Column] public int IdUser { get; set; }
            [Column] public string Username { get; set; }
            [Column] public string FirstName { get; set; }
            [Column] public string LastName { get; set; }
            [Column] public string Email { get; set; }
            [Column] public string LanguageCode { get; set; }
            [Column] public string CultureCode { get; set; }
            [Column] public bool IsEnabled { get; set; }
            [Column] public string GroupNames { get; set; }
            [Column] public string Permissions { get; set; }
        }
    }
}
