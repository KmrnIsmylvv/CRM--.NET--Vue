using Elfo.Round.WriteCycle;

namespace Elfo.Contoso.LearningRoundKamran.Api.SystemManagement.Commands
{
    public class DisableGroupCommand : ICommand
    {
        public short IdGroup { get; set; }
    }
}
