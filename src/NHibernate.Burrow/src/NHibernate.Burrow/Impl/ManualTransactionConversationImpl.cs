using System;
using System.Collections.Generic;

namespace NHibernate.Burrow.Impl
{
    internal class ManualTransactionConversationImpl : AbstractConversation
    {
        private readonly ITransactionManager transactionManager;

        public ManualTransactionConversationImpl() : base()
        {
            IList<ITransaction> transactions = new List<ITransaction>();
            foreach (SessionManager sm in sessManagers.Values)
            {
                transactions.Add(sm.Transaction);
            }
            transactionManager = new TransactionManagerImpl(transactions);
            transactionManager.RolledBack +=new System.EventHandler(TransactionRolledBack);
        }

        private void TransactionRolledBack(object sender, EventArgs e)
        {
           RollbackAndClose();   
        }

        public override ITransactionManager TransactionManager
        {
            get { return transactionManager; }
        }

        protected override SessionManager CreateSessionManager(PersistenceUnit pu)
        {
            return new SessionManager(pu, false);
        }
    }
}