using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Entities.Mapper.Id
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public class QueryParameterData
    {
        private String flatEntityPropertyName;
        private Object value;

        public QueryParameterData(String flatEntityPropertyName, Object value)
        {
            this.flatEntityPropertyName = flatEntityPropertyName;
            this.value = value;
        }

        public String getProperty(String prefix)
        {
            if (prefix != null)
            {
                return prefix + "." + flatEntityPropertyName;
            }
            else
            {
                return flatEntityPropertyName;
            }
        }

        public Object getValue()
        {
            return value;
        }

        public void SetParameterValue(IQuery query)
        {
            query.SetParameter(flatEntityPropertyName, value);
        }

        public String GetQueryParameterName()
        {
            return flatEntityPropertyName;
        }

        public bool Equals(Object o) {
        if (this == o) return true;
        if (!(o is QueryParameterData)) return false;

        QueryParameterData that = (QueryParameterData) o;

        if (flatEntityPropertyName != null ? !flatEntityPropertyName.Equals(that.flatEntityPropertyName) : that.flatEntityPropertyName != null)
            return false;

        return true;
    }

        public int GetHashCode()
        {
            return (flatEntityPropertyName != null ? flatEntityPropertyName.GetHashCode() : 0);
        }
    }
}
