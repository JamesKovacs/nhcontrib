using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;

namespace NHibernate.JetDriver
{
	/// <summary>
	/// JetDbCommand is just a wrapper class for OleDbCommand with special handling of datatypes needed when storing
	/// data into the Access database.
	/// These type conversion are performed in command parameters:
	/// 1) DateTime, Time and Date parameters are converted to string using 'dd-MMM-yyyy HH:mm:ss' format.
	/// 2) Int64 parameter is converted to Int32, possibly throwing an exception.
	/// 
	/// Because of the diference between the way how NHibernate defines identity columns and how Access does, I have to
	/// incorporate another dirty hack here. Because NHibernate does not always use its driver to generate commands 
	/// (ie. in Schema creation classes), this functionality has to be moved "down" to the IDbCommand object. 
	/// IMO, everything in NHibernate should call db queries using its drivers, but at the present time it is not true.
	/// If it was, we could move the replacing functionality up to the driver class, where it's more appropriate, although it
	/// is still a dirty hack :)
	/// 
	/// <p>
	/// Author: <a href="mailto:lukask@welldatatech.com">Lukas Krejci</a>
	/// </p>
	/// </summary>
	public sealed class JetDbCommand : DbCommand
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(JetDbCommand));

		private JetDbConnection _connection;
		private JetDbTransaction _transaction;
        private readonly OleDbCommand _command;
        private readonly List<IDataParameter> _convertedDateParameters = new List<IDataParameter>();

		public static string IdentitySpecPlaceHolder
		{
			get { return "!!! REPLACE THE PRECEEDING 'INT' AND THIS PLACEHOLDER WITH 'COUNTER' !!!"; }
		}

		internal OleDbCommand Command
		{
			get { return _command; }
		}

		public JetDbCommand()
		{
			_command = new OleDbCommand();
		}

		public JetDbCommand(string cmdText, OleDbConnection connection, OleDbTransaction transaction)
		{
			_command = new OleDbCommand(cmdText, connection, transaction);
		}

		public JetDbCommand(string cmdText, OleDbConnection connection)
		{
			_command = new OleDbCommand(cmdText, connection);
		}

		public JetDbCommand(string cmdText)
		{
			_command = new OleDbCommand(cmdText);
		}

		internal JetDbCommand(OleDbCommand command)
		{
			_command = command;
		}

		/// <summary>
		/// So far, the only data type I know about that causes Access to fail everytime is DateTime.
		/// The solution to the problem is to convert the date to a string representing it.
		/// </summary>
		private void CheckParameters()
		{
			if (Command.Parameters.Count == 0) return;

			foreach (IDataParameter p in Command.Parameters)
			{
				if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
					continue;
				switch (p.DbType)
				{
					case DbType.DateTime:
					case DbType.Time:
					case DbType.Date:
						if (p.Value != DBNull.Value)
						{
							p.DbType = DbType.String;
							p.Value = GetNormalizedDateValue((DateTime)p.Value);
                            AddToConvertedDate(p);
						}
						break;
                    case DbType.String:
                        if(_convertedDateParameters.Contains(p))
                        {
                            //Someimes two pass conversion makes a parameter value
                            //of type DateTime to be of String Dbtype
                            DateTime date = DateTime.Parse(p.Value.ToString());
                            p.Value = GetNormalizedDateValue(date);
                        }
                        break;
					case DbType.Int64:
						if (p.Value != DBNull.Value)
						{
							p.DbType = DbType.Int32;
							int normalizedLongValue = Convert.ToInt32((long)p.Value);
							Log.DebugFormat("Changing Int64 parameter value to [{0}] as Int32, to avoid DB confusion", normalizedLongValue);
							p.Value = normalizedLongValue;
						}
				        break;
				}
			}
		}

	    private void AddToConvertedDate(IDataParameter parameter)
	    {
	        _convertedDateParameters.Add(parameter);
	    }

	    private string GetNormalizedDateValue(DateTime date)
        {
            string normalizedDateValue = date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            Log.DebugFormat("Changing datetime parameter value to [{0}] as string, to avoid DB confusion", normalizedDateValue);

            return normalizedDateValue;
        }

		public override void Cancel()
		{
			Command.Cancel();
		}

		public override void Prepare()
		{
			Command.Prepare();
		}

		public override CommandType CommandType
		{
			get { return Command.CommandType; }
			set { Command.CommandType = value; }
		}

	    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
	    {
	        CheckParameters();
	        return Command.ExecuteReader(behavior);
	    }

        //IDataReader IDbCommand.ExecuteReader()
        //{
            
        //}

	    //public IDataReader ExecuteReader(CommandBehavior behavior)
        //{
        //    CheckParameters();
        //    return Command.ExecuteReader(behavior);
        //}

        //IDataReader IDbCommand.ExecuteReader()
        //{
        //    CheckParameters();
        //    return Command.ExecuteReader();
        //}

		public override object ExecuteScalar()
		{
			CheckParameters();
			return Command.ExecuteScalar();
		}

		public override int ExecuteNonQuery()
		{
			CheckParameters();
			Command.CommandText = Command.CommandText.Replace("INT " + IdentitySpecPlaceHolder, "COUNTER");
			return Command.ExecuteNonQuery();
		}

		public override int CommandTimeout
		{
			get { return Command.CommandTimeout; }
			set { Command.CommandTimeout = value; }
		}

	    protected override DbParameter CreateDbParameter()
	    {
	        return Command.CreateParameter();
	    }

	    protected override DbConnection DbConnection
	    {
            get { return _connection; }
	        set 
            {
                _connection = (JetDbConnection) value;
                Command.Connection = _connection.Connection;
            }
	    }

		public override UpdateRowSource UpdatedRowSource
		{
			get { return Command.UpdatedRowSource; }
			set { Command.UpdatedRowSource = value; }
		}

		public override string CommandText
		{
			get { return Command.CommandText; }
			set { Command.CommandText = value; }
		}

	    protected override DbParameterCollection DbParameterCollection
	    {
            get { return Command.Parameters; }
	    }

	    protected override DbTransaction DbTransaction
	    {
            get { return _transaction; }
	        set
	        {
	            if(value == null)
	            {
	                _transaction = null;
	                Command.Transaction = null;
	            }
	            else
	            {
	                _transaction = (JetDbTransaction) value;
                    Command.Transaction = _transaction.Transaction;
	            }
	        }
	    }

	    public override bool DesignTimeVisible
	    {
            get { return Command.DesignTimeVisible; }
            set { Command.DesignTimeVisible = value; }
	    }

	    protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                Command.Dispose();
                _convertedDateParameters.Clear();                
            }

            base.Dispose(disposing);
        }
	}
}
