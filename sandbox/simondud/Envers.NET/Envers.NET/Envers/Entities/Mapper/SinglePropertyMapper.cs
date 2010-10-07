using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Properties;
using NHibernate.Envers.Tools;
using NHibernate.Envers.Exceptions;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Configuration;
using NHibernate.Collection;

namespace NHibernate.Envers.Entities.Mapper
{
    /**
     * TODO Adam Warsky: diff
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    //TODO Simon.
    public class SinglePropertyMapper : IPropertyMapper, ISimpleMapperBuilder {
        private PropertyData propertyData;
        private static readonly List<System.Type> primitiveTypesList = new List<System.Type>{ typeof(bool), typeof(byte), 
                        typeof(char), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double)};

        public SinglePropertyMapper(PropertyData propertyData) {
            this.propertyData = propertyData;
        }

        public SinglePropertyMapper() { }

        public void Add(PropertyData propertyData) {
            if (this.propertyData != null) {
                throw new AuditException("Only one property can be added!");
            }

            this.propertyData = propertyData;
        }

        public bool MapToMapFromEntity(ISessionImplementor session, IDictionary<string, object> data, object newObj, object oldObj) 
        {
            data.Add(propertyData.Name, newObj);
            if (newObj == null)
            {
                return oldObj != null;
            }
            return !newObj.Equals(oldObj);
        }

        public void MapToEntityFromMap(AuditConfiguration verCfg, Object obj, IDictionary<string,object> data, Object primaryKey,
                                       IAuditReaderImplementor versionsReader, long revision) {
            if (data == null || obj == null) {
                return;
            }

            // ORIG: Setter setter = ReflectionTools.getSetter(obj.getClass(), propertyData);
            PropertyInfo propInfo = obj.GetType().GetProperty(propertyData.Name);

            Object value = data[propertyData.Name];
		    // We only set a null value if the field is not primitive. Otherwise, we leave it intact.
            //ORIG: if (value != null || !isPrimitive(setter, propertyData, obj.GetType())) {
            //    setter.Set(obj, value);
		    if (value != null || !isPrimitive(propInfo)) {
        	    propInfo.SetValue(obj, value, null);
		    }
        }

	    private bool isPrimitive(PropertyInfo propInfo) {
		    if (propInfo == null) {
			    throw new HibernateException("No field found for property: " + propInfo.Name);
		    }


            return primitiveTypesList.Contains(propInfo.PropertyType);
	    }

        public IList<PersistentCollectionChangeData> MapCollectionChanges(String referencingPropertyName,
                                                                         IPersistentCollection newColl,
                                                                         Object oldColl,
                                                                         Object id) {
            return null;
        }

    }
}
