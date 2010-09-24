using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Envers.Tools.Query;

namespace NHibernate.Envers.Entities.Mapper.Relation.Component
{
    /**
     * A component mapper for the @MapKey mapping with the name parameter specified: the value of the map's key
     * is a property of the entity. This doesn't have an effect on the data stored in the versions tables,
     * so <code>mapToMapFromObject</code> is empty.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class MiddleMapKeyPropertyComponentMapper : IMiddleComponentMapper {
        private readonly String propertyName;
        private readonly String accessType;

        public MiddleMapKeyPropertyComponentMapper(String propertyName, String accessType) {
            this.propertyName = propertyName;
            this.accessType = accessType;
        }

        public Object MapToObjectFromFullMap(EntityInstantiator entityInstantiator, 
                    IDictionary<String, Object> data,
                    Object dataObject, long revision) {
            // dataObject is not null, as this mapper can only be used in an index.
            //ORIG: return ReflectionTools.getGetter(dataObject.getClass(), propertyName, accessType).get(dataObject);
            return dataObject.GetType().GetProperty(propertyName).GetValue(dataObject, null);
        }

        public void MapToMapFromObject(IDictionary<String, Object> data, Object obj) {
            // Doing nothing.
        }

        public void AddMiddleEqualToQuery(Parameters parameters, String prefix1, String prefix2) {
            // Doing nothing.
        }
    }
}
