using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
	public class ListCollectionMapper<T> : AbstractCollectionMapper<T> 
	{
		private MiddleComponentData _elementComponentData;
		private MiddleComponentData _indexComponentData;

		public ListCollectionMapper(CommonCollectionMapperData commonCollectionMapperData,
									MiddleComponentData elementComponentData, 
									MiddleComponentData indexComponentData) 
						: base(commonCollectionMapperData, typeof(List<>), typeof(ListProxy<>))
		{
			_elementComponentData = elementComponentData;
			_indexComponentData = indexComponentData;
		}

		protected override ICollection GetNewCollectionContent(IPersistentCollection newCollection)
		{
			if (newCollection == null)
			{
				return null;
			}
			return (ICollection) Toolz.ListToIndexElementPairList<T>((IList)newCollection);
		}

		protected override ICollection GetOldCollectionContent(object oldCollection)
		{
			if (oldCollection == null)
			{
				return null;
			}
			return (ICollection) Toolz.ListToIndexElementPairList<T>((IList)oldCollection);
		}

		protected override void MapToMapFromObject(IDictionary<string, object> data, object changed)
		{
			var indexValuePair = (Pair<int, T>)changed;
			_elementComponentData.ComponentMapper.MapToMapFromObject(data, indexValuePair.Second);
			_indexComponentData.ComponentMapper.MapToMapFromObject(data, indexValuePair.First);
		}

		protected override IInitializor<T> GetInitializor(AuditConfiguration verCfg, IAuditReaderImplementor versionsReader, object primaryKey, long revision)
		{
			return new ListCollectionInitializor<T>(verCfg, 
												versionsReader, 
												commonCollectionMapperData.QueryGenerator,
												primaryKey, 
												revision, 
												_elementComponentData, 
												_indexComponentData);
		}
	}
}