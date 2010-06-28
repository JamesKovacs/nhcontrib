using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Entities.Mapper.Id;
using NHibernate.Engine;
using Iesi.Collections;
using NHibernate.Util;
using NHibernate.Envers.Tools;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Reader;
using NHibernate.Properties;
using System.Runtime.Serialization;
using NHibernate.Collection;
using System.Reflection;
using System.Runtime.Remoting;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
/**
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */

    public class ToOneIdMapper : IPropertyMapper
    {
        private IIdMapper delegat;
        private PropertyData propertyData;
        private String referencedEntityName;
        private bool nonInsertableFake;

        public ToOneIdMapper(IIdMapper delegat, PropertyData propertyData, String referencedEntityName, bool nonInsertableFake)
        {
            this.delegat = delegat;
            this.propertyData = propertyData;
            this.referencedEntityName = referencedEntityName;
            this.nonInsertableFake = nonInsertableFake;
        }

        public bool MapToMapFromEntity(ISessionImplementor session, IDictionary<String, Object> data, Object newObj, Object oldObj)
        {
            //Simon 27/06/2010 - era new LinkedHashMap
            IDictionary<String, Object> newData = new Dictionary<String, Object>();
            data.Add(propertyData.Name, newData);

            // If this property is originally non-insertable, but made insertable because it is in a many-to-one "fake"
            // bi-directional relation, we always store the "old", unchaged data, to prevent storing changes made
            // to this field. It is the responsibility of the collection to properly update it if it really changed.
            delegat.MapToMapFromEntity(newData, nonInsertableFake ? oldObj : newObj);

            //noinspection SimplifiableConditionalExpression
            return nonInsertableFake ? false : !Toolz.EntitiesEqual(session, newObj, oldObj);
        }

        public void MapToEntityFromMap(AuditConfiguration verCfg, Object obj, IDictionary<String, Object> data, Object primaryKey,
                                       IAuditReaderImplementor versionsReader, long revision)
        {
            if (obj == null)
            {
                return;
            }

            Object entityId = delegat.MapToIdFromMap((IDictionary<String, Object>)data[propertyData.Name]);
            Object value;
            if (entityId == null)
            {
                value = null;
            }
            else
            {
                if (versionsReader.FirstLevelCache.Contains(referencedEntityName, revision, entityId))
                {
                    value = versionsReader.FirstLevelCache[referencedEntityName, revision, entityId];
                }
                else
                {
                    //java: Class<?> entityClass = ReflectionTools.loadClass(referencedEntityName); 
                    value = versionsReader.SessionImplementor.Factory.GetEntityPersister(referencedEntityName).CreateProxy(
                        entityId, new ToOneDelegateSessionImplementor(versionsReader, Toolz.ResolveDotnetType(referencedEntityName), entityId, revision, verCfg));
                }
            }
            PropertyInfo propInfo = obj.GetType().GetProperty(propertyData.Name);
            propInfo.SetValue(obj, value, null);
        }

        public IList<PersistentCollectionChangeData> MapCollectionChanges(String referencingPropertyName,
                                                                         IPersistentCollection newColl,
                                                                         Object oldColl,
                                                                         Object id)
        {
            return null;
        }


    }
}
