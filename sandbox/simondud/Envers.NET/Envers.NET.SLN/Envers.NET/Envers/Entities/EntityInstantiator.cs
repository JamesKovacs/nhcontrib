using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Entities.Mapper.Id;
using System.Reflection;
using NHibernate.Envers.Exceptions;
using NHibernate.Util;
using System.Collections;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Entities
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    //TODO Simon
    public class EntityInstantiator {
        private readonly AuditConfiguration verCfg;
        private readonly IAuditReaderImplementor versionsReader;

        public EntityInstantiator(AuditConfiguration verCfg, IAuditReaderImplementor versionsReader) {
            this.verCfg = verCfg;
            this.versionsReader = versionsReader;
        }

        /**
         * Creates an entity instance based on an entry from the versions table.
         * @param entityName Name of the entity, which instances should be read
         * @param versionsEntity An entry in the versions table, from which data should be mapped.
         * @param revision Revision at which this entity was read.
         * @return An entity instance, with versioned properties set as in the versionsEntity map, and proxies
         * created for collections.
         */
        public Object createInstanceFromVersionsEntity(String entityName, IDictionary<string,object> versionsEntity, long revision) {
            if (versionsEntity == null) {
                return null;
            }

            // The $type$ property holds the name of the (versions) entity
            String name = verCfg.getEntCfg().getEntityNameForVersionsEntityName(((String) versionsEntity["$type$"]));

            if (name != null) {
                entityName = name;
            }

            // First mapping the primary key
            IIdMapper idMapper = verCfg.getEntCfg()[entityName].GetIdMapper();
            IDictionary<string,object> originalId = (IDictionary<string,object>) versionsEntity[verCfg.getAuditEntCfg().OriginalIdPropName];

            Object primaryKey = idMapper.MapToIdFromMap(originalId);

            // Checking if the entity is in cache
            if (versionsReader.FirstLevelCache.Contains(entityName, revision, primaryKey)) {
                return versionsReader.FirstLevelCache[entityName, revision, primaryKey];
            }

            // If it is not in the cache, creating a new entity instance
            Object ret;
            try {
                //System.Type cls = ReflectionTools.loadClass(entityName);
                //ret = ReflectHelper.GetDefaultConstructor(cls).newInstance();
                ret = Activator.CreateInstance(Toolz.ResolveDotnetType(entityName));
                
            } catch (Exception e) {
                throw new AuditException(e);
            }

            // Putting the newly created entity instance into the first level cache, in case a one-to-one bidirectional
            // relation is present (which is eagerly loaded).
            versionsReader.FirstLevelCache.Add(entityName, revision, primaryKey, ret);

            verCfg.getEntCfg()[entityName].PropertyMapper.MapToEntityFromMap(verCfg, ret, versionsEntity, primaryKey,
                    versionsReader, revision);
            idMapper.MapToEntityFromMap(ret, originalId);

            return ret;
        }

        public void AddInstancesFromVersionsEntities(String entityName, IList addTo, IList<IDictionary<string,object>> versionsEntities, long revision) {
            foreach (IDictionary<string,object> versionsEntity in versionsEntities) {
                addTo.Add(createInstanceFromVersionsEntity(entityName, versionsEntity, revision));
            }
        }
    }
}
