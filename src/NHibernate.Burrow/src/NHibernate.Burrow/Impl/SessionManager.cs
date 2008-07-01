using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// Handlers creation and management of sessions and transactions. 
    /// </summary>
    /// <remarks>
    /// Lifespan : conversation
    /// </remarks>
    internal sealed class SessionManager
    {
        #region fields

        private readonly PersistenceUnit persistenceUnit;

        private readonly ITransaction transaction;
        private bool isDisposing = false;
        private ISession session;
 

        private FlushMode flushMode = FlushMode.Auto;

        #endregion

        #region constructors

        internal SessionManager(PersistenceUnit pu)
        { 
            persistenceUnit = pu;

            transaction = new TransactionImpl();
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
                session.FlushMode = flushMode;
              
            }else if(!session.IsConnected)
            {
                session.Reconnect();
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
        public void CommitAndClose()
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
                GetSession().Flush();
                transaction.Commit();
                
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


        public void SetFlushMode(FlushMode fm)
        {
            this.flushMode = fm;
            if(session != null)
                session.FlushMode = fm;
        }


        #endregion

        #region private members

        public ISessionFactory SessionFactory
        {
            get { return PersistenceUnit.SessionFactory; }
        }

       

        private void CheckDisposed() {
            if (isDisposing)
            {
                throw new GeneralException("SessionManager already disposed");
            }
        }


        #endregion

        public void CommitAndDisconnect()
        {
            if(session != null && session.IsConnected)
            {
               if( Transaction.InTransaction ) //if session is never used, there could be no transaction started in this request
                    Transaction.Commit();
               session.Disconnect();
            }
        }

        public void Disconnect()
        {
            if (session != null && session.IsConnected)
            { 
                session.Disconnect();
            }
        }
    }
}