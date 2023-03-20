using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class DisableUserCommand : ICommand<string>
    {
        public int IdUser { get; set; }
    }
}
