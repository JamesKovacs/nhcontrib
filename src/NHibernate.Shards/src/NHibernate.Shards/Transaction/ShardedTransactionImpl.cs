using System;
using System.Collections.Generic;
using System.Data;
using log4net;

namespace NHibernate.Shards.Transaction
{
	public class ShardedTransactionImpl : IShardedTransaction
	{
		private readonly ILog log = LogManager.GetLogger(typeof (ShardedTransactionImpl));

		private readonly List<ITransaction> transactions;

		private bool begun;
		private bool commitFailed;
		private bool committed;
		private bool rolledBack;
		//private IList<Synchronization> synchronizations;
		private int timeout;
		private bool timeoutSet;

		#region IShardedTransaction Members

		/// <summary>
		/// If a new Session is opened while ShardedTransaction exists, this method is
		/// called with the Session as argument. Implementations should set up a
		/// transaction for this session and sync it with ShardedTransaction
		/// </summary>
		/// <param name="session">The Session on which the Transaction should be setup</param>
		public void SetupTransaction(ISession session)
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Begin the transaction with the default isolation level.
		///            
		///</summary>
		///
		public void Begin()
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Begin the transaction with the specified isolation level.
		///            
		///</summary>
		///
		///<param name="isolationLevel">Isolation level of the transaction</param>
		public void Begin(IsolationLevel isolationLevel)
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Flush the associated 
		///<c>ISession</c> and end the unit of work.
		///            
		///</summary>
		///
		///<remarks>
		///
		///            This method will commit the underlying transaction if and only if the transaction
		///            was initiated by this object.
		///            
		///</remarks>
		///
		public void Commit()
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Force the underlying transaction to roll back.
		///            
		///</summary>
		///
		public void Rollback()
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Enlist the <see cref="T:System.Data.IDbCommand" /> in the current Transaction.
		///            
		///</summary>
		///
		///<param name="command">The <see cref="T:System.Data.IDbCommand" /> to enlist.</param>
		///<remarks>
		///
		///            It is okay for this to be a no op implementation.
		///            
		///</remarks>
		///
		public void Enlist(IDbCommand command)
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///            Is the transaction in progress
		///            
		///</summary>
		///
		public bool IsActive
		{
			get { throw new NotImplementedException(); }
		}

		///<summary>
		///
		///            Was the transaction rolled back or set to rollback only?
		///            
		///</summary>
		///
		public bool WasRolledBack
		{
			get { throw new NotImplementedException(); }
		}

		///<summary>
		///
		///            Was the transaction successfully committed?
		///            
		///</summary>
		///
		///<remarks>
		///
		///            This method could return <see langword="false" /> even after successful invocation of 
		///<c>Commit()</c>
		///</remarks>
		///
		public bool WasCommitted
		{
			get { throw new NotImplementedException(); }
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}