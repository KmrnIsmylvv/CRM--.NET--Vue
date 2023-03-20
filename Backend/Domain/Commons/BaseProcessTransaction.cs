namespace Elfo.Contoso.LearningRoundKamran.Domain
{
    public class BaseProcessTransaction<ProcessStatusType> where ProcessStatusType : BaseProcessStatus
    {
        public BaseProcessTransaction(ProcessStatusType processStatusFrom, ProcessStatusType processStatusTo, byte idProcessTransaction, string desc)
        {
            IdProcessTransaction = idProcessTransaction;
            ProcessStatusTo = processStatusTo;
            ProcessStatusFrom = processStatusFrom;
            TransactionDesc = desc;
        }

        public byte IdProcessTransaction { get; set; }
        public ProcessStatusType ProcessStatusTo { get; set; }
        public ProcessStatusType ProcessStatusFrom { get; set; }
        public string TransactionDesc { get; set; }
    }
}
