using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow.Impl {
    /// <summary>
    /// Handlers creation and management of sessions and transactions. 
    /// </summary>
    /// <remarks>
    /// Lifespan : application, Threadsafe
    /// </remarks>
    internal sealed class SessionManager {
        #region fields

        private readonly object lockObj2 = new object();
        private readonly PersistenceUnit persistenceUnit;
        private bool isDisposing = false;

        #region conversational transaction and session repo

        private static readonly ConversationalData<IDictionary<SessionManager, ISession>> _sessionRepo =
            new ConversationalData<IDictionary<SessionManager, ISession>>(ConversationalDataMode.Overspan,
                                                                          new Dictionary<SessionManager, ISession>());

        private static readonly ConversationalData<IDictionary<SessionManager, ITransaction>> _transactionRepo =
            new ConversationalData<IDictionary<SessionManager, ITransaction>>(ConversationalDataMode.Overspan,
                                                                              new Dictionary
                                                                                  <SessionManager, ITransaction>()
                );

        private IDictionary<SessionManager, ISession> sessionRepo {
            get {
                if (_sessionRepo.Value == null)
                    return _sessionRepo.Value = new Dictionary<SessionManager, ISession>();
                return _sessionRepo.Value;
            }
        }

        private IDictionary<SessionManager, ITransaction> transactionRepo {
            get {
                if (_transactionRepo.Value == null)
                    return _transactionRepo.Value = new Dictionary<SessionManager, ITransaction>();
                return _transactionRepo.Value;
            }
        }

        private ISession threadSession {
            get {
                IDictionary<SessionManager, ISession> repo = sessionRepo;
                if (!repo.ContainsKey(this)) return null;
                return repo[this];
            }
            set { sessionRepo[this] = value; }
        }

        private ITransaction threadTransaction {
            get {
                IDictionary<SessionManager, ITransaction> repo = transactionRepo;
                if (!repo.ContainsKey(this)) return null;
                return repo[this];
            }
            set { transactionRepo[this] = value; }
        }

        #endregion

        #endregion

        #region public properties

        /// <summary>
        /// The PersistenceUnit it belongs to.
        /// </summary>
        public PersistenceUnit PersistenceUnit {
            get { return persistenceUnit; }
        }

        #endregion

        #region static helper members

        /// <summary>
        /// The singleton Instance 
        /// </summary>
        public static SessionManager GetInstance()
        {
            return PersistenceUnitRepo.Instance.GetOnlyPU().SessionManager;
        }

        public static SessionManager GetInstance(System.Type t) {
            return PersistenceUnitRepo.Instance.GetPU(t).SessionManager; ;
        }
 

        /// <summary>
        /// Shortcut to flush the session
        /// </summary>
        public static void Flush() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.GetSession().Flush();
        }

        /// <summary>
        /// Shotcut to clear all the sessions
        /// </summary>
        public static void ClearSessions() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.GetSession().Clear();
        }

        /// <summary>
        /// Begin Transactions for all SessionManagers in All PersistenceUnits
        /// </summary>
        internal static void BeginNHibernateTransactions() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.BeginTransaction();
        }

        /// <summary>
        /// Commit Transactions for all SessionManagers in All PersistenceUnits
        /// </summary>
        internal static void CommitNHibernateTransactions() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.CommitTransaction();
        }

        /// <summary>
        /// Rollback the Transaction and CloseSession
        /// </summary>
        /// <remarks>
        /// if the tranasaction has already been rollback or the session closed this will do nothing. 
        /// You can perform this method multiple times, only the first time will take effect. 
        /// </remarks>
        internal static void RollbackNHibernateTransacitons() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.RollbackTransaction();
        }

        internal static void CloseNHibernateSessions() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                pu.SessionManager.CloseSession();
        }

        #endregion

        #region public methods

        /// <summary>
        /// get a un managed session
        /// </summary>
        /// <returns></returns>
        public ISession GetUnManagedSession() {
            if (isDisposing)
                throw new GeneralException("SessionManager already disposed");
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// Get a managed Session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession() {
            //Todo: exploit a better solution for injecting Interceptor
            return GetSession(null);
        }

        /// <summary>
        /// Close the session.
        /// </summary>
        /// <remarks>
        /// if the session is already closed, this will do nothing
        /// </remarks>
        public void CloseSession() {
            if (threadSession == null) return;
            if (threadSession.IsOpen)
                threadSession.Close();
            threadSession = null;
            threadTransaction = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginTransaction() {
            if (threadTransaction == null)
                threadTransaction = GetSession().BeginTransaction();
        }

        /// <summary>
        /// Try commit the transaction, if failed the transaction will be rollback and the session will be close
        /// </summary>
        public void CommitTransaction() {
            try {
                if (threadTransaction != null && !threadTransaction.WasCommitted && !threadTransaction.WasRolledBack) {
                    if (threadSession == null)
                        throw new GeneralException("The threadSession is null, the possible reasons includes: "
                                                   +
                                                   "The CloseSession Method is called before CommistTransaction Method.");
                    if (!threadSession.IsOpen)
                        throw new GeneralException(
                            "The threadSession was closed by something other than the SessionManger. Do not close session directly. Always use the SessionManager.CloseSession() method ");

                    threadSession.Flush();
                    threadTransaction.Commit();
                }
            }
            catch (Exception) {
                try {
                    RollbackTransaction();
                }
                catch (Exception e) {
                    //Catch the exception thrown from RollBackTransaction() to prevent the original exception from being swallowed.

                    ILog log = LogManager.GetLogger(typeof (SessionManager));
                    if (log.IsErrorEnabled)
                        log.Error("NHibernate.Burrow Rollback failed", e);
                    else Console.WriteLine(e);
                }
                throw;
            }
            finally {
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
        public void RollbackTransaction() {
            try {
                if (threadSession != null && threadSession.IsOpen && threadTransaction != null
                    && !threadTransaction.WasCommitted && !threadTransaction.WasRolledBack)
                    threadTransaction.Rollback();
            }
            finally {
                threadTransaction = null;

                CloseSession();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            lock (lockObj2) {
                if (isDisposing) return;
                isDisposing = true;
            }
            if (threadTransaction != null) CommitTransaction();

            //CloseSession();
        }

        #endregion

        #region constructors

        internal SessionManager(PersistenceUnit pu) {
            persistenceUnit = pu;
        }

        #endregion

        #region private members

        private ISessionFactory SessionFactory {
            get { return PersistenceUnit.SessionFactory; }
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor) {
          
            if (threadSession == null)
                if (interceptor != null)
                    threadSession = SessionFactory.OpenSession(interceptor);
                else
                    threadSession = SessionFactory.OpenSession();

            return threadSession;
        }

        #endregion
    }
}