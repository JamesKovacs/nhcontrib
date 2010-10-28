using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
	public sealed class BasicCollectionMapper<T> : AbstractCollectionMapper<T>
    {
        private readonly MiddleComponentData elementComponentData;

        public BasicCollectionMapper(CommonCollectionMapperData commonCollectionMapperData,
                                    System.Type collectionType, 
									System.Type proxyType,
                                    MiddleComponentData elementComponentData) 
            :base(commonCollectionMapperData, collectionType, proxyType)
        {    
            this.elementComponentData = elementComponentData;
        }

        protected override IInitializor<T> GetInitializor(AuditConfiguration verCfg, 
														IAuditReaderImplementor versionsReader,
														object primaryKey, 
														long revision) 
        {
            return new BasicCollectionInitializor<T>(verCfg, 
											versionsReader, 
											commonCollectionMapperData.QueryGenerator,
											primaryKey, 
											revision, 
											collectionType, 
											elementComponentData,
											GenericArguments);
        }

        protected override ICollection GetNewCollectionContent(IPersistentCollection newCollection) 
        {
            return  (ICollection) newCollection;
        }

		protected override ICollection GetOldCollectionContent(object oldCollection)
        {
            if (oldCollection == null) 
            {
                return null;
            }
            var oldCollAsDic = oldCollection as IDictionary;
            if (oldCollAsDic != null)
            {
                return oldCollAsDic.Keys;
            }
            return (ICollection) oldCollection;
        }

        protected override void MapToMapFromObject(IDictionary<string, object> data, object changed) 
		{
            elementComponentData.ComponentMapper.MapToMapFromObject(data, changed);
        }
    }
}
