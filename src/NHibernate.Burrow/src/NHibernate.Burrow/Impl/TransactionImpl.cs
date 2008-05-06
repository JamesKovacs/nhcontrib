using System;
using System.Collections.Generic;
using log4net;

namespace NHibernate.Burrow.Impl
{
    internal class TransactionImpl : ITransaction
    {
        private NHibernate.ITransaction nhtransaction;
        private SessionManager sessManager;
        
        public TransactionImpl(SessionManager sessManager) {
            this.sessManager = sessManager;
        }

        #region ITransaction Members

        /// <summary>
        /// 
        /// </summary>
        public void Begin()
        {
            if (InTransaction)
            {
                throw new Exceptions.IncorrectTransactionStatusException("Transaction has already begun");
            }
            nhtransaction = sessManager.GetSession().BeginTransaction();

        }
        /// <summary>
        /// whether the transaction has begun and not committed yet
        /// </summary>
        public bool InTransaction
        {
            get{ return nhtransaction != null; }
        }

        /// <summary>
        /// Try commit the nhtransaction, if failed the nhtransaction will be rollback and the session will be close
        /// </summary>
        public void Commit()
        {
            CheckInTransaction();

            try
            {
                if ( !nhtransaction.WasCommitted && !nhtransaction.WasRolledBack)
                {
                    nhtransaction.Commit();
                }
            }
            catch (Exception)
            {
                try
                {
                    Rollback();
                }
                catch (Exception e)
                {
                    //Catch the exception thrown from RollBackTransaction() to prevent the original exception from being swallowed.

                    ILog log = LogFactory.Log;
                    if (log.IsErrorEnabled)
                    {
                        log.Error("NHibernate.Burrow Rollback failed", e);
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                throw;
            }
            finally
            {
                nhtransaction = null;
            }
        }

        private void CheckInTransaction() {
            if(!InTransaction)
                throw new Exceptions.IncorrectTransactionStatusException("It's not in transaction. Either you haven't started it or it's already close");
        }

        /// <summary>
        /// Rollback the Transaction and Close Session
        /// </summary>
        /// <remarks>
        /// if the tranasaction has already been rollback or the session closed this will do nothing. 
        /// You can perform this method multiple times, only the first time will take effect. 
        /// </remarks>
        public void Rollback()
        {
            try
            {
                if (nhtransaction != null && !nhtransaction.WasCommitted && !nhtransaction.WasRolledBack)
                {
                    nhtransaction.Rollback();
                }
            } catch (Exception e)
            {
                //Catch the exception thrown from  to prevent the original exception from being swallowed.
                try
                {
                    ILog log = LogManager.GetLogger(typeof (SessionManager));
                    if (log.IsErrorEnabled)
                    {
                        log.Error("NHibernate.Burrow Rollback failed", e);
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception) {}
            }
            finally
            {
                nhtransaction = null;
            }
        }

     

        #endregion
    }
}