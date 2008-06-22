using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Impl
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
        public static readonly TransactionStrategy NonAtomicConversation = new LongDBTransactionStrategy();
        
        internal abstract void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms);
        internal abstract void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionManager> sms);
        
        
        private class SeparateDBTransactions : TransactionStrategy
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

        private class BusinessTransactionStrategy : SeparateDBTransactions
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionManager> sms)
            {
                foreach (SessionManager sm in sms)
                {
                    sm.SetFlushMode(FlushMode.Never);
                }
            }
        }
 
        private class LongDBTransactionStrategy : TransactionStrategy
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

     

        
    }
 


}