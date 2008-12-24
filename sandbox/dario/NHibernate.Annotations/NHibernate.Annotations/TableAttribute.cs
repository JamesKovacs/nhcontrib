using NHibernate;
using NHibernate.Annotations;

namespace System.Persistence
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TableAttribute : Attribute, INameable
	{
		private string name;
		
		public SQLInsertAttribute SqlInsert { get; set; }
		public SQLDeleteAttribute SqlDelete { get; set; }
		public SQLUpdateAttribute SqlUpdate { get; set; }

		/// <summary>
		///  If enabled, NHibernate will insert a row only if the properties defined by this join are non-null
		/// and will always use an outer join to retrieve the properties.
		/// <remarks>
		/// Only applies to secondary tables
		/// </remarks>
		/// </summary>
		public bool IsOptional { get; set; }

		/// <summary>
		/// If true, Hibernate will not try to insert or update the properties defined by this join.
		/// <remarks>
		/// Only applies to secondary tables
		/// </remarks>
		/// </summary>
		public bool IsInverse { get; set; }

		/// <summary>
		/// If set to Join, the default, NHibernate will use an inner join to retrieve a
		/// secondary table defined by a class or its superclasses and an outer join for a
		/// secondary table defined by a subclass.
		/// If set to select then NHibernate will use a
		/// sequential select for a secondary table defined on a subclass, which will be issued only if a row
		/// turns out to represent an instance of the subclass. Inner joins will still be used to retrieve a
		/// secondary defined by the class and its superclasses.
		/// <remarks>
		/// Only applies to secondary tables
		/// </remarks>
		/// </summary>
		public FetchMode Fetch { get; set; }

		//string catalog = string.Empty;
		//string schema =string.Empty;
		//UniqueConstraint[] uniqueConstraints;

		/**
	 * name of the targeted table
	 */
		public string AppliesTo { get; set; }

		#region INameable Members

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion

		///**
		// * Indexes
		// */
		//Index[] indexes() default {};

		/// <summary>
		/// define a table comment
		/// </summary>
		public string Comment { get; set;}

		
		/// <summary>
		/// Defines the Foreign Key name of a secondary table
		///  pointing back to the primary table
		/// </summary>
		public ForeignKeyAttribute ForeignKey { get; set;} 

		///**
		// * Defines a custom SQL insert statement
		// *
		// * <b>Only applies to secondary tables</b>
		// */
		//SQLInsert sqlInsert() default @SQLInsert(sql="");

		///**
		// * Defines a custom SQL update statement
		// *
		// * <b>Only applies to secondary tables</b>
		// */
		//SQLUpdate sqlUpdate() default @SQLUpdate(sql="");

		///**
		// * Defines a custom SQL delete statement
		// *
		// * <b>Only applies to secondary tables</b>
		// */
		//SQLDelete sqlDelete() default @SQLDelete(sql="");
	}
}