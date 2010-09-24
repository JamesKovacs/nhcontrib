using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public abstract class AbstractCollectionMapper<T> : IPropertyMapper {
        protected readonly CommonCollectionMapperData commonCollectionMapperData;    
        protected readonly System.Type collectionType;

        private readonly ConstructorInfo proxyConstructor;

        protected AbstractCollectionMapper(CommonCollectionMapperData commonCollectionMapperData,
                                           System.Type collectionType, System.Type proxyType) {
            this.commonCollectionMapperData = commonCollectionMapperData;
            this.collectionType = collectionType;

            try {
                proxyConstructor = proxyType.GetConstructor(new System.Type[]{typeof(IInitializor<>)});
            } catch (ArgumentException e) {
                throw new AuditException(e);
            }
        }

        protected abstract ICollection<object> GetNewCollectionContent(IPersistentCollection newCollection);
        protected abstract ICollection<object> GetOldCollectionContent(object oldCollection);

        /**
         * Maps the changed collection element to the given map.
         * @param data Where to map the data.
         * @param changed The changed collection element to map.
         */
        protected abstract void MapToMapFromObject(IDictionary<String, Object> data, Object changed);

        private void addCollectionChanges(IList<PersistentCollectionChangeData> collectionChanges, 
                            ISet<Object> changed, RevisionType revisionType, object id) {
            foreach (Object changedObj in changed) {
                IDictionary<String, Object> entityData = new Dictionary<String, Object>();
                IDictionary<String, Object> originalId = new Dictionary<String, Object>();
                entityData.Add(commonCollectionMapperData.VerEntCfg.OriginalIdPropName, originalId);

                collectionChanges.Add(new PersistentCollectionChangeData(
                        commonCollectionMapperData.VersionsMiddleEntityName, entityData, changedObj));
                // Mapping the collection owner's id.
                commonCollectionMapperData.ReferencingIdData.PrefixedMapper.MapToMapFromId(originalId, id);

                // Mapping collection element and index (if present).
                MapToMapFromObject(originalId, changedObj);

                entityData.Add(commonCollectionMapperData.VerEntCfg.RevisionTypePropName, revisionType);
            }
        }

        public IList<PersistentCollectionChangeData> MapCollectionChanges(String referencingPropertyName,
                                                                         IPersistentCollection newColl,
                                                                         object oldColl, object id) {
            if (!commonCollectionMapperData.CollectionReferencingPropertyData.Name
                    .Equals(referencingPropertyName)) {
                return null;
            }

            IList<PersistentCollectionChangeData> collectionChanges = new List<PersistentCollectionChangeData>();

            // Comparing new and old collection content.
            ICollection<object> newCollection = GetNewCollectionContent(newColl);
            ICollection<object> oldCollection = GetOldCollectionContent(oldColl);

            ISet<Object> added = new HashedSet<Object>();
            if (newColl != null) { added.AddAll(newCollection); }
		    // Re-hashing the old collection as the hash codes of the elements there may have changed, and the
		    // removeAll in AbstractSet has an implementation that is hashcode-change sensitive (as opposed to addAll).
            if (oldColl != null) { added.RemoveAll(new HashedSet<object>(oldCollection)); }

            addCollectionChanges(collectionChanges, added, RevisionType.ADD, id);

            Set<Object> deleted = new HashedSet<Object>();
            if (oldColl != null) { deleted.AddAll(oldCollection); }
		    // The same as above - re-hashing new collection.
            if (newColl != null) { deleted.RemoveAll(new HashedSet<object>(newCollection)); }

            addCollectionChanges(collectionChanges, deleted, RevisionType.DEL, id);

            return collectionChanges;
        }

        public bool MapToMapFromEntity(ISessionImplementor session, IDictionary<String, Object> data, Object newObj, Object oldObj) {
            // Changes are mapped in the "mapCollectionChanges" method.
            return false;
        }

        protected abstract IInitializor<T> GetInitializor(AuditConfiguration verCfg,
                                                         IAuditReaderImplementor versionsReader, Object primaryKey,
                                                         long revision);

        public void MapToEntityFromMap(AuditConfiguration verCfg, Object obj, IDictionary<string, object> data, 
                            Object primaryKey, IAuditReaderImplementor versionsReader, long revision) {
            //ORIG: Setter setter = ReflectionTools.getSetter(obj.getClass(),
            //        commonCollectionMapperData.CollectionReferencingPropertyData);
            PropertyInfo propInfo = obj.GetType().GetProperty(
                commonCollectionMapperData.CollectionReferencingPropertyData.Name);

            try {
                //TODO Simon - see if have to catch all exceptions thrown by Invoke
                propInfo.SetValue(obj, proxyConstructor.Invoke(
                    new object[]{GetInitializor(verCfg, versionsReader, primaryKey, revision)}), null);
            } catch (InstantiationException e) {
                throw new AuditException(e);
            } catch (SecurityException e) {
                throw new AuditException(e);
            } catch (TargetInvocationException e) {
                throw new AuditException(e);
            }
        }
    }
}
