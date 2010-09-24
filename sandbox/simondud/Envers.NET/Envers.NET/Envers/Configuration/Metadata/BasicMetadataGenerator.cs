using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NHibernate.Envers.Configuration.Metadata.Reader;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Envers.Configuration.Metadata;
using NHibernate.Envers.Tools.Graph;
using NHibernate.Envers.Entities;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Type;
using Iesi.Collections.Generic;
using log4net;



namespace NHibernate.Envers.Configuration.Metadata
{
    /**
     * Generates metadata for basic properties: immutable types (including enums).
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class BasicMetadataGenerator 
    {
        public bool AddBasic(XmlElement parent, PropertyAuditingData propertyAuditingData,
					 IValue value, ISimpleMapperBuilder mapper, bool insertable, bool key) {
            NHibernate.Type.IType  type = value.Type;
                
		    if (type is ImmutableType || type is MutableType) {
			    AddSimpleValue(parent, propertyAuditingData, value, mapper, insertable, key);
		    } else if (type is CustomType || type is CompositeCustomType) {
			    AddCustomValue(parent, propertyAuditingData, value, mapper, insertable, key);
            }
            // TODO Simon: There is no equivalent of PrimitiveByteArrayBlobType in NHibernate, will see later if needed
            // ORIG:
            //else if ("org.hibernate.type.PrimitiveByteArrayBlobType".equals(type.getClass().getName()))
            //{
            //    AddSimpleValue(parent, propertyAuditingData, value, mapper, insertable, key);
            //}
		    else {
			    return false;
		    }

		    return true;
	    }

            private void AddSimpleValue(XmlElement parent, PropertyAuditingData propertyAuditingData,
                                    IValue value, ISimpleMapperBuilder mapper, bool insertable, bool key)
            {
		    if (parent != null) {
                XmlElement prop_mapping = MetadataTools.AddProperty(parent, propertyAuditingData.Name,
                        value.Type.Name, propertyAuditingData.ForceInsertable || insertable, key);
                MetadataTools.AddColumns(prop_mapping, (IEnumerator<ISelectable>)value.ColumnIterator.GetEnumerator());
		    }

		    // A null mapper means that we only want to add xml mappings
		    if (mapper != null) {
			    mapper.Add(propertyAuditingData.getPropertyData());
		    }
	    }

	    private void AddCustomValue(XmlElement parent, PropertyAuditingData propertyAuditingData,
								    IValue value, ISimpleMapperBuilder mapper, bool insertable, bool key) {
		    if (parent != null) {
			    XmlElement prop_mapping = MetadataTools.AddProperty(parent, propertyAuditingData.Name,
					    null, insertable, key);

			    //CustomType propertyType = (CustomType) value.getType();

			    XmlElement type_mapping = parent.OwnerDocument.CreateElement("type");
                prop_mapping.AppendChild(type_mapping);
			    type_mapping.SetAttribute("name", value.GetType().Name);

			    if (value is SimpleValue) {
	                IDictionary<string, string> typeParameters = ((SimpleValue)value).TypeParameters;
				    if (typeParameters != null) {
					    foreach (KeyValuePair<string,string> paramKeyValue in typeParameters) {
                            XmlElement type_param = parent.OwnerDocument.CreateElement("param");
						    type_param.SetAttribute("name", (String) paramKeyValue.Key);
						    type_param["name"].Value =  paramKeyValue.Value;
					    }
				    }
			    }

                MetadataTools.AddColumns(prop_mapping, (IEnumerator<ISelectable>)value.ColumnIterator);
		    }

		    if (mapper != null) {
			    mapper.Add(propertyAuditingData.getPropertyData());
		    }
	    }
    }
}
