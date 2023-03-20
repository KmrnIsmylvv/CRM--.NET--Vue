using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
	public class CreateUserCommand : ICommand<int>
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string LanguageCode { get; set; }
		public string CultureCode { get; set; }
		public string TelephoneNumber { get; set; }
		public IEnumerable<short> IdGroupList { get; set; }
		public IEnumerable<string> Permissions { get; set; }
	}
}
