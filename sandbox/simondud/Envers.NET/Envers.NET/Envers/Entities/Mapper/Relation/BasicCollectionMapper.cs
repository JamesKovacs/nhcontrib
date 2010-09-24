using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Collection;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
    /**
     * @author Adam Warski (adam at warski dot org)
     * TODO Simon in lucru
     */
    public sealed class BasicCollectionMapper<T>: AbstractCollectionMapper<T>, IPropertyMapper {
        private readonly MiddleComponentData elementComponentData;

        public BasicCollectionMapper(CommonCollectionMapperData commonCollectionMapperData,
                                     System.Type collectionType, System.Type proxyType,
                                     MiddleComponentData elementComponentData) 
            :base(commonCollectionMapperData, collectionType, proxyType)
        {    
            this.elementComponentData = elementComponentData;
        }

        protected override IInitializor<T> GetInitializor(AuditConfiguration verCfg, IAuditReaderImplementor versionsReader,
                                                Object primaryKey, long revision) {
            return new BasicCollectionInitializor<T>(verCfg, versionsReader, commonCollectionMapperData.QueryGenerator,
                    primaryKey, revision, collectionType, elementComponentData);
        }

        protected override ICollection<object> GetNewCollectionContent(IPersistentCollection newCollection) {
            return (ICollection<object>) newCollection;
        }

        protected override ICollection<object> GetOldCollectionContent(object oldCollection)
        {
            if (oldCollection == null) {
                return null;
            } else if (oldCollection is IDictionary) {
                return ((IDictionary<object, object>) oldCollection).Keys;
            } else {
                return (ICollection<object>)oldCollection;
            }
        }

        protected override void MapToMapFromObject(IDictionary<String, Object> data, Object changed) {
            elementComponentData.ComponentMapper.MapToMapFromObject(data, changed);
        }
    }
}
