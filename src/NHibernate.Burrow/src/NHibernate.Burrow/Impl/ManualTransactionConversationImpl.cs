using System;
using System.Collections.Generic;

namespace NHibernate.Burrow.Impl
{
    internal class ManualTransactionConversationImpl : AbstractConversation
    {
        private readonly ITransactionManager transactionManager;

        public ManualTransactionConversationImpl() : base()
        {
            
            transactionManager = new TransactionManagerImpl(this);
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

        protected override TransactionStrategy TransactionStrategy
        {
            get { return TransactionStrategy.ManualTransaction; }
            set
            {
                
            }
        }

      
    }
}