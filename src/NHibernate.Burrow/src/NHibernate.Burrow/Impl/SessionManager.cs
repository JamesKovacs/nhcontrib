using System;
using System.Collections.Generic;
using log4net;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// Handlers creation and management of sessions and transactions. 
    /// </summary>
    /// <remarks>
    /// Lifespan : application, Threadsafe
    /// </remarks>
    internal sealed class SessionManager
    {
        #region fields

        private readonly PersistenceUnit persistenceUnit;
        private readonly ITransactionManager transactionManager;
        private bool isDisposing = false;

        #region conversational transaction and session repo

        private readonly IDictionary<SessionManager, ISession> sessionRepo = new Dictionary<SessionManager, ISession>();

        private ISession threadSession
        {
            get
            {
                IDictionary<SessionManager, ISession> repo = sessionRepo;
                if (!repo.ContainsKey(this))
                {
                    return null;
                }
                return repo[this];
            }
            set { sessionRepo[this] = value; }
        }

        #endregion

        #endregion

        #region public properties

        /// <summary>
        /// The PersistenceUnit it belongs to.
        /// </summary>
        public PersistenceUnit PersistenceUnit
        {
            get { return persistenceUnit; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// get a un managed session
        /// </summary>
        /// <returns></returns>
        public ISession GetUnManagedSession()
        {
            if (isDisposing)
            {
                throw new GeneralException("SessionManager already disposed");
            }
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// Get a managed Session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            //Todo: exploit a better solution for injecting Interceptor
            return GetSession(null);
        }

        /// <summary>
        /// Close the session.
        /// </summary>
        /// <remarks>
        /// if the session is already closed, this will do nothing
        /// </remarks>
        public void CloseSession()
        {
            if (threadSession == null)
            {
                return;
            }
            if (threadSession.IsOpen)
            {
                threadSession.Close();
            }
            threadSession = null;
            transactionManager.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction()
        {
            transactionManager.BeginTransaction(GetSession());
        }

        /// <summary>
        /// Try commit the transaction, if failed the transaction will be rollback and the session will be close
        /// </summary>
        public void CommitTransaction()
        {
            if (threadSession == null)
            {
                throw new GeneralException("The threadSession is null, the possible reasons includes: "
                                           + "The CloseSession Method is called before CommistTransaction Method.");
            }
            if (!threadSession.IsOpen)
            {
                throw new GeneralException(
                    "The threadSession was closed by something other than the SessionManger. Do not close session directly. Always use the SessionManager.CloseSession() method ");
            }

            try
            {
                threadSession.Flush();
            }
            catch (Exception)
            {
                RollBackWithoutExceptionThrown();
                throw;
            }
            transactionManager.CommitTransaction();
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
                if (threadSession != null && threadSession.IsOpen)
                {
                    transactionManager.RollbackTransaction();
                }
            }
            finally
            {
                CloseSession();
            }
        }

        public void RollBackWithoutExceptionThrown()
        {
            try
            {
                RollbackTransaction();
            }
            catch (Exception e)
            {
                //Catch the exception thrown from RollBackTransaction() to prevent the original exception from being swallowed.
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
        }

        #endregion

        #region constructors

        internal SessionManager(PersistenceUnit pu)
        {
            persistenceUnit = pu;
            if (pu.Configuration.ManualTransactionManagement)
            {
                transactionManager = new VoidTransactionManager();
            }
            else
            {
                transactionManager = new TransactionManagerImpl();
            }
        }

        #endregion

        #region private members

        private ISessionFactory SessionFactory
        {
            get { return PersistenceUnit.SessionFactory; }
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor)
        {
            if (threadSession == null)
            {
                if (interceptor != null)
                {
                    threadSession = SessionFactory.OpenSession(interceptor);
                }
                else
                {
                    threadSession = SessionFactory.OpenSession();
                }
            }

            return threadSession;
        }

        #endregion
    }
}