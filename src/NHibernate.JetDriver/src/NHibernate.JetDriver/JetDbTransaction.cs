using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace NHibernate.JetDriver
{
	/// <summary>
	/// Summary description for JetDbTransaction.
	/// </summary>
	public sealed class JetDbTransaction : DbTransaction
	{
		private readonly OleDbTransaction _transaction;
		private readonly JetDbConnection _connection;

		internal OleDbTransaction Transaction
		{
			get { return _transaction; }
		}

		internal JetDbTransaction(JetDbConnection connection, OleDbTransaction transaction)
		{
			_connection = connection;
			_transaction = transaction;
		}

	    protected override DbConnection DbConnection
	    {
            get { return _connection; }
	    }

	    public override IsolationLevel IsolationLevel
	    {
	        get { return Transaction.IsolationLevel; }
	    }

	    public override void Commit()
	    {
	        Transaction.Commit();
	    }

	    public override void Rollback()
	    {
	        Transaction.Rollback();
	    }
	}
}