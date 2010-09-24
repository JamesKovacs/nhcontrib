using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Util;
using NHibernate.Properties;
using NHibernate.Envers.Exceptions;
using System.Reflection;

namespace NHibernate.Envers.Entities.Mapper.Id
{

/**
 * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
 */
public class EmbeddedIdMapper  : AbstractCompositeIdMapper , ISimpleIdMapperBuilder
    {
        private PropertyData idPropertyData;

        public EmbeddedIdMapper(PropertyData idPropertyData, System.Type compositeIdClass)
        : base(compositeIdClass)
        {
            this.idPropertyData = idPropertyData;
        }

        public override void MapToMapFromId(IDictionary<string, object> data, Object obj)
        {
        foreach (IIdMapper idMapper in ids.Values) {
            idMapper.MapToMapFromEntity(data, obj);
        }
    }

        public override void MapToMapFromEntity(IDictionary<string, object> data, Object obj)
        {
            if (obj == null)
            {
                return;
            }
            PropertyInfo propInfo;
            //IGetter getter = ReflectionTools.getGetter(obj.getClass(), idPropertyData);
            propInfo = obj.GetType().GetProperty(idPropertyData.Name);
            MapToMapFromId(data, propInfo.GetValue(obj,null));
        }

        public override void MapToEntityFromMap(Object obj, IDictionary<string, object> data)
        {
        if (data == null || obj == null) {
            return;
        }
        PropertyInfo propInfo = obj.GetType().GetProperty(idPropertyData.Name);

        try {
            Object subObj = ReflectHelper.GetDefaultConstructor(propInfo.ReflectedType); //getter.getReturnType()).newInstance();
           
            propInfo.SetValue(obj, subObj, null);

            foreach(IIdMapper idMapper in ids.Values) {
            idMapper.MapToEntityFromMap(subObj, data);
            }
        } catch (Exception e) {
            throw new AuditException(e);
        }
    }

        public override IIdMapper PrefixMappedProperties(String prefix)
        {
        EmbeddedIdMapper ret = new EmbeddedIdMapper(idPropertyData, compositeIdClass);

        foreach (PropertyData propertyData in ids.Keys) {
            String propertyName = propertyData.Name;
            ret.ids.Add(propertyData, new SingleIdMapper(new PropertyData(prefix + propertyName, propertyData)));
        }

        return ret;
    }

        public override Object MapToIdFromEntity(object data)
        {
            if (data == null)
            {
                return null;
            }

            PropertyInfo propInfo = data.GetType().GetProperty(idPropertyData.Name);
            return propInfo.GetValue(data, null);
        }

        public override IList<QueryParameterData> MapToQueryParametersFromId(object obj) {
            //Simon 27/06/2010 - era LinkedHashMap
            IDictionary<String, Object> data = new Dictionary<String, Object>();
            MapToMapFromId(data, obj);

            IList<QueryParameterData> ret = new List<QueryParameterData>();

            foreach (KeyValuePair<String, Object> propertyData in data) {
                ret.Add(new QueryParameterData(propertyData.Key, propertyData.Value));
            }

            return ret;
        }

    }
}
