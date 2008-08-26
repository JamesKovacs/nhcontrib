using System.Collections.Generic;
using NHibernate.Annotations;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
	public class IndexOrUniqueKeySecondPass : ISecondPass
	{
		private Table table;
		private readonly string indexName;
		private readonly string[] columns;
		private readonly ExtendedMappings mappings;
		private readonly Ejb3Column column;
		private readonly bool unique;

		public IndexOrUniqueKeySecondPass(Table table, string indexName, string[] columns, ExtendedMappings mappings)
		{
			this.table = table;
			this.indexName = indexName;
			this.columns = columns;
			this.mappings = mappings;
			column = null;
			unique = false;
		}

		public IndexOrUniqueKeySecondPass(string indexName, Ejb3Column column, ExtendedMappings mappings)
			: this(indexName, column, mappings, false)
		{
		}

		public IndexOrUniqueKeySecondPass(string indexName, Ejb3Column column, ExtendedMappings mappings, bool unique)
		{
			this.indexName = indexName;
			this.column = column;
			columns = null;
			this.mappings = mappings;
			this.unique = unique;
		}
        
		public void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses)
		{
			if (columns != null)
			{
				foreach (var columnName in columns)
				{
					AddConstraintToColumn(columnName);
				}
			}
			if (column != null)
			{
				table = column.Table;
				AddConstraintToColumn(mappings.GetLogicalColumnName(column.Name, table));
			}
		}

		private void AddConstraintToColumn(string columnName)
		{
			Column column = table.GetColumn(new Column(mappings.GetPhysicalColumnName(columnName, table)));
			
			if (column == null)
				throw new AnnotationException("@Index references a unknown column: " + columnName);
			
			if (unique)
				table.GetOrCreateUniqueKey(indexName).AddColumn(column);
			else
				table.GetOrCreateIndex(indexName).AddColumn(column);
		}
	}
}