using System;
using System.Collections.Generic;
using System.Persistence;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.NHibernate.Cfg
{
	public class Ejb3JoinColumn : Ejb3Column
	{
		public static Ejb3JoinColumn BuildJoinColumn(PrimaryKeyJoinColumnAttribute pkJoinAnn,
									JoinColumnAttribute joinAnn, 
									IKeyValue identifier,
									IDictionary<string, Join> joins, 
									IPropertyHolder propertyHolder, 
									ExtendedMappings mappings)
		{
			throw new NotImplementedException();
		}
	}
}