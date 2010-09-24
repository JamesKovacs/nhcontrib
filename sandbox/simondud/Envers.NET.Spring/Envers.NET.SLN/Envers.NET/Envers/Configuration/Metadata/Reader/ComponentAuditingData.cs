using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Configuration.Metadata.Reader
{
    /**
     * Audit mapping meta-data for component.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class ComponentAuditingData : PropertyAuditingData, IAuditedPropertiesHolder {
	    private IDictionary<String, PropertyAuditingData> properties;

	    public ComponentAuditingData() {
		    this.properties = new Dictionary<String, PropertyAuditingData>();
	    }

	    public void addPropertyAuditingData(String propertyName, PropertyAuditingData auditingData) {
		    properties.Add(propertyName, auditingData);
	    }

        public PropertyAuditingData getPropertyAuditingData(String propertyName) {
            return properties[propertyName];
        }
    }
}
