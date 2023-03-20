using Elfo.Round.WriteCycle;
using System.Collections.Generic;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
	public class UpdateGroupCommand : ICommand
	{
		public short IdGroup { get; set; }
		public string Name { get; set; }
		public IEnumerable<int> IdUserList { get; set; }
		public IEnumerable<string> Permissions { get; set; }
	}
}
