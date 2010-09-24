using System;
using Spring.Transaction.Support;

namespace NHibernate.Envers.Synchronization
{
    public class EnversTransactionSynchronization : ITransactionSynchronization
    {
        private AuditSync sync;
        public EnversTransactionSynchronization(AuditSync _sync)
        {
            sync = _sync;
        }

        public void Suspend()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void BeforeCommit(bool readOnly)
        {
            throw new NotImplementedException();
        }

        public void AfterCommit()
        {
            throw new NotImplementedException();
        }

        public void BeforeCompletion()
        {
            sync.BeforeCompletion();
        }

        public void AfterCompletion(TransactionSynchronizationStatus status)
        {
            sync.AfterCompletion(true);
        }
    }
}