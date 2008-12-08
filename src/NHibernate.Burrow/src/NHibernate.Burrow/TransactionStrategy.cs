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
        


        internal abstract void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionAndTransactionManager> sms);
        internal abstract void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionAndTransactionManager> sms);
        internal abstract void OnConversationEnds(IEnumerable<SessionAndTransactionManager> sms);
        internal abstract void OnSessionUsed(SessionAndTransactionManager sm);

		internal abstract void ChangeFlushModeOnConversationStopsSpan(IEnumerable<SessionAndTransactionManager> sms);
		 
        private abstract class ManagedTransaction : TransactionStrategy
        {
            internal sealed override void OnConversationEnds(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.CommitAndClose();
                }
            }

            internal sealed override void OnSessionUsed(SessionAndTransactionManager sm)
            {
                if(!sm.Transaction.InTransaction)
                    sm.Transaction.Begin(sm.GetSession());
            }
        }


        private class TransactionWithWorkSpaceStrategy : ManagedTransaction
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionAndTransactionManager> sms)
            { }

            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.CommitAndDisconnect();
                }
            }

			internal override void ChangeFlushModeOnConversationStopsSpan(IEnumerable<SessionAndTransactionManager> sms)
			{

			}
        	
        }

        private class BusinessTransactionStrategy : TransactionWithWorkSpaceStrategy
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.SetFlushMode(FlushMode.Never);
                }
            }

			internal override void ChangeFlushModeOnConversationStopsSpan(IEnumerable<SessionAndTransactionManager> sms)
			{
				foreach (SessionAndTransactionManager sm in sms)
				{
					FlushMode fm = sm.GetFlushMode();
					if (fm != FlushMode.Never)
						throw new Exceptions.IllegalFlushModeException(
							fm + " is illegal in business transaction. " +
							"When doing Business transaction, you should not change FlushMode.");
					sm.SetFlushMode(FlushMode.Auto);
				}
			}
        }

        private class LongDBTransactionStrategy : ManagedTransaction
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.SetFlushMode(FlushMode.Auto);
                }
            }

            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionAndTransactionManager> sms)
            {}
			internal override void ChangeFlushModeOnConversationStopsSpan(IEnumerable<SessionAndTransactionManager> sms)
			{

			}
        }

        private class ManualTransactionStrategy : TransactionStrategy
        {
            internal override void ChangeFlushModeOnConversationBeginsSpan(IEnumerable<SessionAndTransactionManager> sms)
            {}
			internal override void ChangeFlushModeOnConversationStopsSpan(IEnumerable<SessionAndTransactionManager> sms)
			{}
            internal override void OnWorkSpaceClosedBeforeConversationEnds(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.Disconnect();
                }
            }

            internal override void OnConversationEnds(IEnumerable<SessionAndTransactionManager> sms)
            {
                foreach (SessionAndTransactionManager sm in sms)
                {
                    sm.CloseSession();
                }
            }

            internal override void OnSessionUsed(SessionAndTransactionManager sm)
            {
            }
        }

    	
    }
}