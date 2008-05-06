using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Impl
{
    internal class AtomicConversationImpl : AbstractConversation, IConversation
    {
        protected override SessionManager CreateSessionManager(PersistenceUnit pu)
        {
            return new SessionManager(pu, true);
        }

        /// <summary>
        /// This is not support in AtomicConversation, will throw an Exception
        /// </summary>
        public override ITransactionManager TransactionManager
        {
            get { throw new NotSupportedException("Manual transaction management is not supported in Atomic Conversation." 
                        +" To use manual transaction please see BurrowFramework.InitStickyWorkSpace()"); }
        }
    }
}
