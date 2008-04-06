using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// a transaction manager that does no transaction management
    /// </summary>
    /// <remarks>
    /// this is for in manual transaction mode, so that client can control the transaction itself.
    /// </remarks>
    internal class VoidTransactionManager : ITransactionManager {
      
        public void BeginTransaction(ISession sess) {
            return;
        }
 
        public void CommitTransaction() {
            return;
        }

       
        public void RollbackTransaction() {
            return;
        }

        public void Dispose() {
            return;
        }
    }
}
