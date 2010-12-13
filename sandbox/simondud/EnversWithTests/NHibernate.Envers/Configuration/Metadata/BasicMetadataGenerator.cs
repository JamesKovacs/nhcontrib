using System.Collections.Generic;
using System.Xml;
using NHibernate.Envers.Configuration.Metadata.Reader;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Envers.Tools;
using NHibernate.Mapping;
using NHibernate.Type;


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
            var  type = value.Type;
                
		    if (type is ImmutableType || type is MutableType) 
			{
			    AddSimpleValue(parent, propertyAuditingData, value, mapper, insertable, key);
		    } 
			else if (type is CustomType || type is CompositeCustomType) {
			    AddCustomValue(parent, propertyAuditingData, value, mapper, insertable, key);
            }
            // TODO Simon: There is no equivalent of PrimitiveByteArrayBlobType in NHibernate, will see later if needed
            // ORIG:
            //else if ("org.hibernate.type.PrimitiveByteArrayBlobType".equals(type.getClass().getName()))
            //{
            //    AddSimpleValue(parent, propertyAuditingData, value, mapper, insertable, key);
            //}
		    else 
			{
			    return false;
		    }

		    return true;
	    }

        private void AddSimpleValue(XmlElement parent, PropertyAuditingData propertyAuditingData,
                                IValue value, ISimpleMapperBuilder mapper, bool insertable, bool key)
            {
		    if (parent != null) 
			{
                var prop_mapping = MetadataTools.AddProperty(parent, propertyAuditingData.Name,
                        value.Type.Name, propertyAuditingData.ForceInsertable || insertable, key);
                MetadataTools.AddColumns(prop_mapping, value.ColumnIterator.GetEnumerator());
		    }

		    // A null mapper means that we only want to add xml mappings
		    if (mapper != null) 
			{
			    mapper.Add(propertyAuditingData.getPropertyData());
		    }
	    }

	    private void AddCustomValue(XmlElement parent, PropertyAuditingData propertyAuditingData,
								    IValue value, ISimpleMapperBuilder mapper, bool insertable, bool key) 
		{
		    if (parent != null) 
			{
			    var prop_mapping = MetadataTools.AddProperty(parent, propertyAuditingData.Name,
					    null, insertable, key);

		    	var userType = Toolz.ResolveDotnetType(value.Type.Name);
			    prop_mapping.SetAttribute("type", userType.AssemblyQualifiedName);

		    	var simpleValue = value as SimpleValue;

			    if (simpleValue != null) 
				{
					var typeParameters = simpleValue.TypeParameters;
				    if (typeParameters != null) 
					{
					    foreach (var paramKeyValue in typeParameters) 
						{
                            var type_param = parent.OwnerDocument.CreateElement("param");
						    type_param.SetAttribute("name", paramKeyValue.Key);
						    type_param["name"].Value =  paramKeyValue.Value;
					    }
				    }
			    }

                MetadataTools.AddColumns(prop_mapping, (IEnumerator<ISelectable>)value.ColumnIterator.GetEnumerator());
		    }

		    if (mapper != null) 
			{
			    mapper.Add(propertyAuditingData.getPropertyData());
		    }
	    }
    }
}
