using System;
using System.Collections.Generic;
using log4net;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
    internal class TransactionManagerImpl : ITransactionManager {
    

        private  readonly IDictionary<TransactionManagerImpl, ITransaction> transactionRepo =   new Dictionary<TransactionManagerImpl, ITransaction>();
       


        private ITransaction threadTransaction
        {
            get
            {
                IDictionary<TransactionManagerImpl, ITransaction> repo = transactionRepo;
                if (!repo.ContainsKey(this)) return null;
                return repo[this];
            }
            set { transactionRepo[this] = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction(ISession sess)
        {
            if (threadTransaction == null)
                threadTransaction = sess.BeginTransaction();
        }

        /// <summary>
        /// Try commit the transaction, if failed the transaction will be rollback and the session will be close
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (threadTransaction != null && !threadTransaction.WasCommitted && !threadTransaction.WasRolledBack)
                {
                   threadTransaction.Commit();
                }
            }
            catch (Exception)
            {
                try
                {
                    RollbackTransaction();
                }
                catch (Exception e)
                {
                    //Catch the exception thrown from RollBackTransaction() to prevent the original exception from being swallowed.

                    ILog log = LogManager.GetLogger(typeof(SessionManager));
                    if (log.IsErrorEnabled)
                        log.Error("NHibernate.Burrow Rollback failed", e);
                    else Console.WriteLine(e);
                }
                throw;
            }
            finally
            {
                threadTransaction = null;
            }
        }

        /// <summary>
        /// Rollback the Transaction and Close Session
        /// </summary>
        /// <remarks>
        /// if the tranasaction has already been rollback or the session closed this will do nothing. 
        /// You can perform this method multiple times, only the first time will take effect. 
        /// </remarks>
        public void RollbackTransaction()
        {
            try
            {
                if (  threadTransaction != null
                    && !threadTransaction.WasCommitted && !threadTransaction.WasRolledBack)
                    threadTransaction.Rollback();
            }
            finally
            {
                threadTransaction = null;
            }
        }

        public void Dispose() {
            threadTransaction = null;
        }
    }
}