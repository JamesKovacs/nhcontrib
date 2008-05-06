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

        private readonly ITransaction transaction;
        private bool isDisposing = false;
        private ISession session;

        /// <summary>
        /// Whether transaction share the same life cycle as session
        /// </summary>
        private bool transactionWithSession = true;

        #endregion

        #region constructors

        internal SessionManager(PersistenceUnit pu, bool transactionWithSession)
        {
            this.transactionWithSession = transactionWithSession;
            persistenceUnit = pu;
            if (pu.Configuration.ManualTransactionManagement)
            {
                transaction = new VoidTransaction();
            }
            else
            {
                transaction = new TransactionImpl(this);
            }
        }

        #endregion

        public ITransaction Transaction
        {
            get { return transaction; }
        }

        /// <summary>
        /// The PersistenceUnit it belongs to.
        /// </summary>
        public PersistenceUnit PersistenceUnit
        {
            get { return persistenceUnit; }
        }

        #region public methods

        /// <summary>
        /// Get a managed Session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            if (session == null)
            {
                IInterceptor interceptor = PersistenceUnit.CreateInterceptor();
                if (interceptor != null)
                {
                    session = SessionFactory.OpenSession(interceptor);
                }
                else
                {
                    session = SessionFactory.OpenSession();
                }
                if (transactionWithSession)
                {
                    transaction.Begin();
                }
            }

            return session;
        }

        /// <summary>
        /// Close the session.
        /// </summary>
        /// <remarks>
        /// if the session is already closed, this will do nothing
        /// </remarks>
        public void CloseSession()
        {
            if (session == null)
            {
                return;
            }

            if (session.IsOpen)
            {
                if (transaction.InTransaction)
                {
                    transaction.Rollback();
                }
                session.Close();
            }
            session = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnConversationStarts() {}

        /// <summary>
        /// Try commit the transaction, if failed the transaction will be rollback and the session will be close
        /// </summary>
        public void OnConversationFinish()
        {
            if (session == null)
            {
                return; //session is never used
            }
            if (!session.IsOpen)
            {
                throw new GeneralException(
                    "The session was closed by something other than the SessionManger. Do not close session directly. Always use the SessionManager.CloseSession() method ");
            }

            try
            {
                if (transactionWithSession)
                {
                    transaction.Commit();
                }
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// Rollback the Transaction and Close Session
        /// </summary>
        /// <remarks>
        /// if the tranasaction has already been rollback or the session closed this will do nothing. 
        /// You can perform this method multiple times, only the first time will take effect. 
        /// </remarks>
        public void OnConversationRollback()
        {
            try
            {
                if (session != null && session.IsOpen)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                CloseSession();
            }
        }

        #endregion

        #region private members

        private ISessionFactory SessionFactory
        {
            get { return PersistenceUnit.SessionFactory; }
        }

        /// <summary>
        /// get a un managed session
        /// </summary>
        /// <returns></returns>
        public ISession GetUnManagedSession(IInterceptor interceptor)
        {
            if (isDisposing)
            {
                throw new GeneralException("SessionManager already disposed");
            }

            return interceptor != null ? SessionFactory.OpenSession(interceptor) : SessionFactory.OpenSession();
        }

        #endregion
    }
}