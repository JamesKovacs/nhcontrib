using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;

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
	    private const string DATASOURCE_KEY = "Data Source";

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
			_connection = new OleDbConnection(ConvertRelativePath(connectionString));
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
			set { Connection.ConnectionString = ConvertRelativePath(value); }
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

        /// <summary>
        /// Converts relative path to datasource (e.g. '..\data\datafile.mdb') to 
        /// an absolute path.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static string ConvertRelativePath(string connectionString)
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            var datasource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (string) builder[DATASOURCE_KEY]);

            builder[DATASOURCE_KEY] = datasource;

            return builder.ConnectionString;
        }
	}
}