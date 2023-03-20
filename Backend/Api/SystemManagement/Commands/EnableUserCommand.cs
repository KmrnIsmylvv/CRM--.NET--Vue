using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class EnableUserCommand : ICommand<string>
    {
        public int IdUser { get; set; }
    }
}
