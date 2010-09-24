using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Entities
{
    /**
     * Holds information on a property that is audited.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class PropertyData {
        public String Name { get; private set; }
	    /**
	     * Name of the property in the bean.
	     */
        public String BeanName { get; private set; }
        public String AccessType {get; private set;}
        public ModificationStore Store {get; private set;}

        /**
         * Copies the given property data, except the name.
         * @param newName New name.
         * @param propertyData Property data to copy the rest of properties from.
         */
        public PropertyData(String newName, PropertyData propertyData) {
            this.Name = newName;
		    this.BeanName = propertyData.BeanName;
            this.AccessType = propertyData.AccessType;
            this.Store = propertyData.Store;
        }

        /**
         * @param name Name of the property.
	     * @param beanName Name of the property in the bean.
         * @param accessType Accessor type for this property.
         * @param store How this property should be stored.
         */
        public PropertyData(String name, String beanName, String accessType, ModificationStore store) {
            this.Name = name;
		    this.BeanName = beanName;
            this.AccessType = accessType;
            this.Store = store;
        }

	    public override bool Equals(Object o) {
		    if (this == o) return true;
		    if (o == null || GetType() != o.GetType()) return false;

		    PropertyData that = (PropertyData) o;

		    if (AccessType != null ? !AccessType.Equals(that.AccessType) : that.AccessType != null) return false;
		    if (BeanName != null ? !BeanName.Equals(that.BeanName) : that.BeanName != null) return false;
		    if (Name != null ? !Name.Equals(that.Name) : that.Name != null) return false;
		    if (Store != that.Store) return false;

		    return true;
	    }

	    public override int GetHashCode() {
		    int result = Name != null ? Name.GetHashCode() : 0;
		    result = 31 * result + (BeanName != null ? BeanName.GetHashCode() : 0);
		    result = 31 * result + (AccessType != null ? AccessType.GetHashCode() : 0);
		    result = 31 * result + (Store != null ? Store.GetHashCode() : 0);
		    return result;
	    }
    }
}
