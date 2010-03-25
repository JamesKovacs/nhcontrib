using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Exceptions;
using System.Reflection;
using NHibernate.Properties;
using NHibernate.Proxy;

namespace NHibernate.Envers.Entities.Mapper.Id
{
/**
* @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
*/

public class SingleIdMapper : AbstractIdMapper , ISimpleIdMapperBuilder {
    private PropertyData propertyData;

    public SingleIdMapper() {
    }

    public SingleIdMapper(PropertyData propertyData) {
        this.propertyData = propertyData;
    }

    public void Add(PropertyData propertyData) {
        if (this.propertyData != null) {
            throw new AuditException("Only one property can be added!");
        }

        this.propertyData = propertyData;
    }

    public override void MapToEntityFromMap(Object obj, IDictionary<String, Object> data)
    {//IDictionary<PropertyData, SingleIdMapper>  
        if (data == null || obj == null) {
            return;
        }

        PropertyInfo propInfo;
        propInfo = obj.GetType().GetProperty(propertyData.Name);
        propInfo.SetValue(obj, data[propertyData.Name], null);

    }

    public override Object MapToIdFromMap(IDictionary<String, Object> data)
    {  //IDictionary<PropertyData, SingleIdMapper>  
        if (data == null) {
            return null;
        }

        return data[propertyData.Name];
    }

    public override Object MapToIdFromEntity(Object data)
    {
        if (data == null) {
            return null;
        }

        if(data is INHibernateProxy) {
        	INHibernateProxy hibernateProxy = (INHibernateProxy)data;
        	return hibernateProxy.HibernateLazyInitializer.Identifier;
        } else {
            PropertyInfo propInfo = data.GetType().GetProperty(propertyData.Name);
            return propInfo.GetValue(data, null);
        }
    }

    public override void MapToMapFromId(IDictionary<String, Object> data, Object obj)
    {
        if (data != null) {
            data.Add(propertyData.Name,obj);
        }
    }

    public override void MapToMapFromEntity(IDictionary<String, Object> data, Object obj)
    {
        if (obj == null) {
            data.Add(propertyData.Name,null);
        } else {
            if(obj is INHibernateProxy) {
            	INHibernateProxy hibernateProxy = (INHibernateProxy)obj;
            	data.Add(propertyData.Name , hibernateProxy.HibernateLazyInitializer.Identifier);
            } else {
                PropertyInfo propInfo = obj.GetType().GetProperty(propertyData.Name);
                data.Add(propertyData.Name, propInfo.GetValue(obj, null));
            }
        }
    }

    public void MapToEntityFromEntity(Object objTo, Object objFrom) {
        if (objTo == null || objFrom == null) {
            return;
        }
        PropertyInfo propInfoFrom = objFrom.GetType().GetProperty(propertyData.Name);
        PropertyInfo propInfoTo = objTo.GetType().GetProperty(propertyData.Name);
        propInfoTo.SetValue(objTo, propInfoFrom.GetValue(objFrom,null), null);
    }

    public override IIdMapper PrefixMappedProperties(String prefix) {
        return new SingleIdMapper(new PropertyData(prefix + propertyData.Name, propertyData));
    }

    public override IList<QueryParameterData> MapToQueryParametersFromId(Object obj)
    {
        IList<QueryParameterData> ret = new List<QueryParameterData>();

        ret.Add(new QueryParameterData(propertyData.Name, obj));

        return ret;
    }

}

}
