using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Impl
{
    internal class ConversationWithManagedTransactionImpl : AbstractConversation, IConversation
    {
        private readonly static TransactionStrategy defaultTransactionStrategy = TransactionStrategy.TransactionWithWorkSpace;

         TransactionStrategy ts  = defaultTransactionStrategy;
        
        protected override TransactionStrategy TransactionStrategy
        {
            get { return ts; }
            set {
                if (value == null)
                    ts = defaultTransactionStrategy;
                ts = value;
            }
        }
 
        /// <summary>
        /// This is not support in AtomicConversation, will throw an Exception
        /// </summary>
        public override ITransactionManager TransactionManager
        {
            get { throw new NotSupportedException("Manual transaction management is not supported in Atomic Conversation." 
                        +" To use manual transaction please see BurrowFramework.InitWorkSpaceWithManualTransaction()"); }
        }
    }
}
