using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
	/// <summary>
	/// Handlers creation and management of sessions and transactions. 
	/// </summary>
	/// <remarks>
	/// Lifespan : conversation
	/// </remarks>
	internal sealed class SessionAndTransactionManager {
		#region fields

		private readonly PersistenceUnit persistenceUnit;
		private readonly ITransaction transaction;
		private FlushMode flushMode = FlushMode.Auto;
		private ISession session;

		#endregion

		#region constructors

		internal SessionAndTransactionManager(PersistenceUnit pu) {
			persistenceUnit = pu;
			transaction = new TransactionImpl();
		}

		#endregion

		public ITransaction Transaction {
			get { return transaction; }
		}

		/// <summary>
		/// The PersistenceUnit it belongs to.
		/// </summary>
		public PersistenceUnit PersistenceUnit {
			get { return persistenceUnit; }
		}

		#region public methods

		/// <summary>
		/// Get a managed Session
		/// </summary>
		/// <returns></returns>
		public ISession GetSession() {
			if (session == null) {
				IInterceptor interceptor = PersistenceUnit.CreateInterceptor();
				if (interceptor != null)
					session = SessionFactory.OpenSession(interceptor);
				else
					session = SessionFactory.OpenSession();
				session.FlushMode = flushMode;
			}
			else if (!session.IsConnected)
				session.Reconnect();
			return session;
		}

		/// <summary>
		/// Close the session.
		/// </summary>
		/// <remarks>
		/// if the session is already closed, this will do nothing
		/// </remarks>
		public void CloseSession() {
			if (session == null)
				return;

			if (session.IsOpen)
				session.Close();
			session = null;
		}

		/// <summary>
		/// Try commit the transaction and close the session, if failed the transaction will be rollback and the session will be close
		/// </summary>
		public void CommitAndClose() {
			if (session == null)
				return; //session is never used
			if (!session.IsOpen)
				throw new GeneralException(
					"The session was closed by something other than the SessionManger. Do not close session directly. It should be managed by Burrow");

			try {
				if (Transaction.InTransaction)
					Transaction.Commit();
			}
			finally {
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
		public void OnConversationRollback() {
			try {
				if (session != null && session.IsOpen)
					transaction.Rollback();
			}
			finally {
				CloseSession();
			}
		}

		public void SetFlushMode(FlushMode fm) {
			flushMode = fm;
			if (session != null)
				session.FlushMode = fm;
		}

		public FlushMode GetFlushMode() {
			return flushMode;
		}

		#endregion

		#region private members

		public ISessionFactory SessionFactory {
			get { return PersistenceUnit.SessionFactory; }
		}

		#endregion

		public void CommitAndDisconnect() {
			if (session != null && session.IsConnected) {
				if (Transaction.InTransaction) //if session is never used, there could be no transaction started in this request
					Transaction.Commit();
				session.Disconnect();
			}
		}

		public void Disconnect() {
			if (session != null && session.IsConnected)
				session.Disconnect();
		}
	}
}