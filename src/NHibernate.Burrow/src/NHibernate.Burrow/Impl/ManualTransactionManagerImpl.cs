using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Impl
{
    internal class ManualTransactionManagerImpl : ITransactionManager
    {
        private IList<ITransaction> transactions;
        public event System.EventHandler RolledBack;
        public ConversationWithManualTransactionImpl conversation;
        public ManualTransactionManagerImpl(ConversationWithManualTransactionImpl conversation)
        {
            this.conversation = conversation;
        }
    

        /// <summary>
        /// begin transactions
        /// </summary>
        public void Begin()
        {

            transactions = new List<ITransaction>();
            foreach (SessionAndTransactionManager sm in conversation.SessionManagers)
            {
                transactions.Add(sm.Transaction);
                sm.Transaction.Begin(sm.GetSession());
            }
           
        }

        /// <summary>
        /// Commit transactions
        /// </summary>
        public void Commit()
        {
            bool someTransactionHasCommitted = false;
            foreach (ITransaction transaction in transactions)
            {
                try
                {
                    transaction.Commit();
                    someTransactionHasCommitted = true;
                }
                catch (Exception)
                {
                    if(someTransactionHasCommitted)
                        LogFactory.Log.Error("transaction atmoc broke");
                    Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Rollback transactions
        /// </summary>
        public void Rollback()
        {
            foreach (ITransaction transaction in transactions)
            {
               transaction.Rollback();
            }
            if (RolledBack != null)
                RolledBack(this, new EventArgs());
        }
    }
}
