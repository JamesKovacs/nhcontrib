using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace NHibernate.JetDriver
{
	/// <summary>
	/// Wrapper class for OleDbConnection to support MS Access dialect in NHibernate.
	/// 
	/// <p>
	/// Author: <a href="mailto:lukask@welldatatech.com">Lukas Krejci</a>
	/// </p>
	/// </summary>
	public sealed class JetDbConnection : DbConnection
	{
		private readonly OleDbConnection _connection;

		internal OleDbConnection Connection
		{
			get { return _connection; }
		}

		public JetDbConnection()
		{
			_connection = new OleDbConnection();
		}

		public JetDbConnection(string connectionString)
		{
			_connection = new OleDbConnection(connectionString);
		}

		public JetDbConnection(OleDbConnection connection)
		{
			_connection = connection;
		}

	    public override void ChangeDatabase(string databaseName)
	    {
	        Connection.ChangeDatabase(databaseName);
	    }

	    protected override DbTransaction BeginDbTransaction(IsolationLevel il)
	    {
	        return new JetDbTransaction(this, Connection.BeginTransaction(il));
	    }

        //IDbTransaction IDbConnection.BeginTransaction()
        //{
        //    return new JetDbTransaction(this, Connection.BeginTransaction());
        //}

	    public override string ServerVersion
	    {
	        get { return "4.0"; }
	    }

	    public override ConnectionState State
		{
			get { return Connection.State; }
		}

		public override string ConnectionString
		{
			get { return Connection.ConnectionString; }
			set { Connection.ConnectionString = value; }
		}

	    protected override DbCommand CreateDbCommand()
	    {
	        return new JetDbCommand(Connection.CreateCommand());
	    }

	    public override void Open()
		{
			Connection.Open();
		}

		public override void Close()
		{
			Connection.Close();
		}

		public override string Database
		{
			get { return Connection.Database; }
		}

	    public override string DataSource
	    {
	        get { return Connection.DataSource; }
	    }

	    public override int ConnectionTimeout
		{
			get { return Connection.ConnectionTimeout; }
		}

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Connection.Dispose();
            }

 	        base.Dispose(disposing);
        }
	}
}