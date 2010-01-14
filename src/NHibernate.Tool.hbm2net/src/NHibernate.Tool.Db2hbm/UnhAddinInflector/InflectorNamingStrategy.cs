using System;
using NHibernate.Cfg;
using NHibernate.Util;

namespace uNhAddIns.Inflector
{
	public class InflectorNamingStrategy : INamingStrategy
	{
		private readonly IInflector inflector;

		public InflectorNamingStrategy(IInflector inflector)
		{
			if (inflector == null)
			{
				throw new ArgumentNullException("inflector");
			}
			this.inflector = inflector;
		}

		#region Implementation of INamingStrategy

		public virtual string ClassToTableName(string className)
		{
			return inflector.Tableize(StringHelper.Unqualify(className));
		}

		public virtual string PropertyToColumnName(string propertyName)
		{
			return inflector.Unaccent(StringHelper.Unqualify(propertyName));
		}

		public virtual string TableName(string tableName)
		{
			return tableName;
		}

		public virtual string ColumnName(string columnName)
		{
			return columnName;
		}

		public virtual string PropertyToTableName(string className, string propertyName)
		{
			return ClassToTableName(className) + PropertyToColumnName(propertyName);
		}

		public string LogicalColumnName(string columnName, string propertyName)
		{
			// not used
			return columnName;
		}

		#endregion
	}
}