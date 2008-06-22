using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow
{
    /// <summary>
    /// Transaction Strategy for long running conversations
    /// </summary>
    public abstract class TransactionStrategy 
    {
        /// <summary>
        /// Strategy that turns off the AutoFlush and allows separate DB transaction for each request, session will be flush at the end of the conversation
        /// </summary>
        /// <remarks>
        /// In this strategy, you will lose the auto flush before query, you can also mandate a flush calling Session.Flush();
        /// </remarks>
        public static readonly TransactionStrategy BusinessTransaction = new BusinessTransactionStrategy();

        /// <summary>
        /// Strategy that will maintain a long DB transaction with the same life span as the conversation ( so as a DB lock).
        /// </summary>
        /// <remarks>
        /// in this strategy you will have a long DB transaction and connection and a lock with the same life span as the conversation
        /// </remarks>
        public static readonly TransactionStrategy LongDBTransaction = new LongDBTransactionStrategy();
        
        /// <summary>
        /// Conversation will break into different transactions in different request. so the conversation won't be atomic, it simply allows you to have a conversation context
        /// </summary>
        internal static readonly TransactionStrategy TransactionWithWorkSpace = new TransactionWithWorkSpaceStrategy();

        internal static readonly TransactionStrategy ManualTransaction = new ManualTransactionStrategy();
        


        internal abstract void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms);
        internal abstract void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionManager> sms);
        internal abstract void OnConversationEnds(IEnumerable<SessionManager> sms);
        internal abstract void OnSessionUsed(SessionManager sm);

        private abstract class ManagedTransaction : TransactionStrategy
        {
            internal override void OnConversationEnds(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.CommitAndClose();
                }
            }

            internal override void OnSessionUsed(SessionManager sm)
            {
                if(!sm.Transaction.InTransaction)
                    sm.Transaction.Begin(sm.GetSession());
            }
        }


        private class TransactionWithWorkSpaceStrategy : ManagedTransaction
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms)
            { }

            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.CommitAndDisconnect();
                }
            }
        }

        private class BusinessTransactionStrategy : TransactionWithWorkSpaceStrategy
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.SetFlushMode(FlushMode.Never);
                }
            }
        }

        private class LongDBTransactionStrategy : ManagedTransaction
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.SetFlushMode(FlushMode.Auto);
                }
            }

            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionManager> sms)
            {}
        }

        private class ManualTransactionStrategy : TransactionStrategy
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms)
            {}

            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.Disconnect();
                }
            }

            internal override void OnConversationEnds(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.CloseSession();
                }
            }

            internal override void OnSessionUsed(SessionManager sm)
            {
            }
        }

     

        
    }
}