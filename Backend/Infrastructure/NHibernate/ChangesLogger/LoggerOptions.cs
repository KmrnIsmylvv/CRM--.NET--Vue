namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
    public class LoggerOptions
    {
        public string ApplicationName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public bool IsActive { get; set; }
    }
}
