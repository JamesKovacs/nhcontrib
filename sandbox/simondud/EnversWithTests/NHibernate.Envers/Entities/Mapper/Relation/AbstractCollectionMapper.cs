using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using Iesi.Collections;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
    public abstract class AbstractCollectionMapper<T> : IPropertyMapper 
    {
        protected readonly CommonCollectionMapperData commonCollectionMapperData;    
        protected readonly System.Type collectionType;

        private readonly ConstructorInfo proxyConstructor;

        protected AbstractCollectionMapper(CommonCollectionMapperData commonCollectionMapperData,
                                            System.Type collectionType, 
                                            System.Type proxyType) 
        {
            this.commonCollectionMapperData = commonCollectionMapperData;
            this.collectionType = collectionType;

            try 
            {
                proxyConstructor = proxyType.GetConstructor(new[]{typeof(IInitializor<>)});
            } 
            catch (ArgumentException e) 
            {
                throw new AuditException(e);
            }
        }

        protected abstract ICollection GetNewCollectionContent(IPersistentCollection newCollection);
        protected abstract ICollection GetOldCollectionContent(object oldCollection);

        /**
         * Maps the changed collection element to the given map.
         * @param data Where to map the data.
         * @param changed The changed collection element to map.
         */
        protected abstract void MapToMapFromObject(IDictionary<String, Object> data, Object changed);

        private void addCollectionChanges(IList<PersistentCollectionChangeData> collectionChanges, 
                                            ISet changed, 
                                            RevisionType revisionType, 
                                            object id) 
        {
            foreach (var changedObj in changed) 
            {
                var entityData = new Dictionary<String, Object>();
                var originalId = new Dictionary<String, Object>();
                entityData.Add(commonCollectionMapperData.VerEntCfg.OriginalIdPropName, originalId);

                collectionChanges.Add(new PersistentCollectionChangeData(
                        commonCollectionMapperData.VersionsMiddleEntityName, entityData, changedObj));
                // Mapping the collection owner's id.
                commonCollectionMapperData.ReferencingIdData.PrefixedMapper.MapToMapFromId(originalId, id);

                // Mapping collection element and index (if present).
                MapToMapFromObject(originalId, changedObj);

                entityData.Add(commonCollectionMapperData.VerEntCfg.RevisionTypePropName, revisionType.Representation);
            }
        }

        public IList<PersistentCollectionChangeData> MapCollectionChanges(string referencingPropertyName,
                                                                            IPersistentCollection newColl,
                                                                            object oldColl, 
                                                                            object id) 
        {
            if (!commonCollectionMapperData.CollectionReferencingPropertyData.Name.Equals(referencingPropertyName)) 
            {
                return null;
            }

            var collectionChanges = new List<PersistentCollectionChangeData>();

            // Comparing new and old collection content.
            var newCollection = GetNewCollectionContent(newColl);
            var oldCollection = GetOldCollectionContent(oldColl);

            var added = new HashedSet();
            if (newColl != null) { added.AddAll(newCollection); }
		    // Re-hashing the old collection as the hash codes of the elements there may have changed, and the
		    // removeAll in AbstractSet has an implementation that is hashcode-change sensitive (as opposed to addAll).
            if (oldColl != null) { added.RemoveAll(new HashedSet(oldCollection)); }

            addCollectionChanges(collectionChanges, added, RevisionType.ADD, id);

            var deleted = new HashedSet();
            if (oldColl != null) { deleted.AddAll(oldCollection); }
		    // The same as above - re-hashing new collection.
            if (newColl != null) { deleted.RemoveAll(new HashedSet(newCollection)); }

            addCollectionChanges(collectionChanges, deleted, RevisionType.DEL, id);

            return collectionChanges;
        }

        public bool MapToMapFromEntity(ISessionImplementor session, IDictionary<string, object> data, object newObj, object oldObj) 
        {
            // Changes are mapped in the "mapCollectionChanges" method.
            return false;
        }

        protected abstract IInitializor<T> GetInitializor(AuditConfiguration verCfg,
                                                        IAuditReaderImplementor versionsReader, 
                                                        object primaryKey,
                                                        long revision);

        public void MapToEntityFromMap(AuditConfiguration verCfg, 
                                        object obj, 
                                        IDictionary<string, object> data, 
                                        object primaryKey, 
                                        IAuditReaderImplementor versionsReader, 
                                        long revision) 
        {
            //ORIG: Setter setter = ReflectionTools.getSetter(obj.getClass(),
            //        commonCollectionMapperData.CollectionReferencingPropertyData);
            var propInfo = obj.GetType().GetProperty(
                commonCollectionMapperData.CollectionReferencingPropertyData.Name);

            try 
			{
                //TODO Simon - see if have to catch all exceptions thrown by Invoke
                propInfo.SetValue(obj, proxyConstructor.Invoke(
                    new object[]{GetInitializor(verCfg, versionsReader, primaryKey, revision)}), null);
            } 
			catch (InstantiationException e) 
			{
                throw new AuditException(e);
            } 
			catch (SecurityException e) 
			{
                throw new AuditException(e);
            } 
			catch (TargetInvocationException e) 
			{
                throw new AuditException(e);
            }
        }
    }
}
