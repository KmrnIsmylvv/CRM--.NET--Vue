using NHibernate;
using System;

namespace Elfo.Contoso.LearningRoundKamran.Infrastructure.NHibernate.ChangesLogger
{
    public class NHChangesInterceptor : EmptyInterceptor
    {
        public IServiceProvider ServiceProvider { get; }
        public NHChangesInterceptor(IServiceProvider serviceProvider) => ServiceProvider = serviceProvider;
    }
}
