namespace Elfo.Contoso.LearningRoundKamran.Domain
{
    public class BaseProcessStatus
    {
        public BaseProcessStatus(string idProcessStatus, string processStatusDesc, string entityOwner)
        {
            IdProcessStatus = idProcessStatus;
            ProcessStatusDesc = processStatusDesc;
            EntityOwner = entityOwner;
        }

        public string IdProcessStatus { get; set; }
        public string ProcessStatusDesc { get; set; }
        public string EntityOwner { get; set; }

        #region Public Methods

        public override string ToString()
        {
            return IdProcessStatus;
        }

        #endregion
    }
}
