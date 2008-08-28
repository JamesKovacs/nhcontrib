using System;

namespace NHibernate.Annotations
{
	public class SqlAttributeBase : Attribute
	{
		/// <summary>
		/// Procedure name or INSERT STATEMENT
		/// </summary>
		public string Sql { get; set; }

		/// <summary>
		/// Is the statement using stored procedure or not
		/// </summary>
		public bool Callable { get; set; }

		/// <summary>
		/// For persistence operation what style of determining results (success/failure) is to be used.
		/// </summary>
		public ResultCheckStyle Check { get; set; }
	}
}
