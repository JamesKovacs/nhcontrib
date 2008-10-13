using System;
using System.Collections.Generic;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class TableBinder
	{
		public static void BindFk(PersistentClass referencedEntity,
		                          PersistentClass destinationEntity,
		                          Ejb3JoinColumn[] columns,
		                          SimpleValue value,
		                          bool unique,
		                          ExtendedMappings mappings)
		{
			throw new NotImplementedException();
		}

		public static Table FillTable(string schema, string catalog, string realTableName, string logicalName,
		                              bool? isAbstract,
		                              IList<string[]> uniqueConstraints,
		                              string constraints,
		                              Table denormalizedSuperTable,
		                              ExtendedMappings mappings)
		{
			throw new NotImplementedException();
		}
	}
}