using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Envers.Configuration.Metadata.Reader;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Configuration.Metadata;
using NHibernate.Envers.Tools.Graph;
using NHibernate.Properties;
using NHibernate.Type;
using Iesi.Collections.Generic;
using NHibernate.Envers.Entities.Mapper.Id;

namespace NHibernate.Envers.Configuration.Metadata
{
    /**
     * Generates metadata for primary identifiers (ids) of versions entities.
     * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public sealed class IdMetadataGenerator {
    private  AuditMetadataGenerator mainGenerator;

    public IdMetadataGenerator(AuditMetadataGenerator auditMetadataGenerator) {
        mainGenerator = auditMetadataGenerator;
    }
    
    //@SuppressWarnings({"unchecked"})
    private void AddIdProperties(XmlElement parent, IEnumerator<Property> properties, ISimpleMapperBuilder mapper, bool key) {
        while (properties.MoveNext() ) {
            Property property = properties.Current;
            IType propertyType = property.Type;
            if (!"_identifierMapper".Equals(property.Name)) {
                if (propertyType is ImmutableType) {
                    // Last but one parameter: ids are always insertable
                    mainGenerator.BasicMetadataGenerator.AddBasic(parent,
                            GetIdPersistentPropertyAuditingData(property),
                            property.Value, mapper, true, key);
                } else {
                    throw new MappingException("Type not supported: " + propertyType.Name);
                }
            }
        }
    }

    //@SuppressWarnings({"unchecked"})
    public IdMappingData AddId(PersistentClass pc) {
        // Xml mapping which will be used for relations
        XmlDocument id_mappingDoc = new XmlDocument();
        XmlElement rel_id_mapping = id_mappingDoc.CreateElement("properties"); //= new DefaultElement("properties"); // (din DOM4J)
        // Xml mapping which will be used for the primary key of the versions table
        XmlElement orig_id_mapping = id_mappingDoc.CreateElement("composite-id");  //= new DefaultElement("composite-id"); // (din DOM4J)

        Property id_prop = pc.IdentifierProperty;
        Component id_mapper = pc.IdentifierMapper;

        // Checking if the id mapping is supported
        if (id_mapper == null && id_prop == null) {
            return null;
        }

        ISimpleIdMapperBuilder mapper;
        if (id_mapper != null) {
            // Multiple id

            mapper = new MultipleIdMapper(((Component)pc.Identifier).ComponentClass);
            AddIdProperties(rel_id_mapping, (IEnumerator<Property>)id_mapper.PropertyIterator, mapper, false);

            // null mapper - the mapping where already added the first time, now we only want to generate the xml
            AddIdProperties(orig_id_mapping, (IEnumerator<Property>)id_mapper.PropertyIterator, null, true);
        } else if (id_prop.IsComposite) {
            // Embedded id

            Component id_component = (Component) id_prop.Value;

            mapper = new EmbeddedIdMapper(GetIdPropertyData(id_prop), id_component.ComponentClass);
            AddIdProperties(rel_id_mapping, (IEnumerator<Property>)id_component.PropertyIterator, mapper, false);

            // null mapper - the mapping where already added the first time, now we only want to generate the xml
            AddIdProperties(orig_id_mapping, (IEnumerator<Property>)id_component.PropertyIterator, null, true);
        } else {
            // Single id
            
            mapper = new SingleIdMapper();

            // Last but one parameter: ids are always insertable
            mainGenerator.BasicMetadataGenerator.AddBasic(rel_id_mapping,
                    GetIdPersistentPropertyAuditingData(id_prop),
                    id_prop.Value, mapper, true, false);

            // null mapper - the mapping where already added the first time, now we only want to generate the xml
            mainGenerator.BasicMetadataGenerator.AddBasic(orig_id_mapping,
                    GetIdPersistentPropertyAuditingData(id_prop),
                    id_prop.Value, null, true, true);
        }

        orig_id_mapping.SetAttribute("name", mainGenerator.VerEntCfg.OriginalIdPropName);

        // Adding a relation to the revision entity (effectively: the "revision number" property)
        mainGenerator.addRevisionInfoRelation(orig_id_mapping);

        return new IdMappingData(mapper, orig_id_mapping, rel_id_mapping);
    }

    private PropertyData GetIdPropertyData(Property property) {
        return new PropertyData(property.Name, property.Name, property.PropertyAccessorName,
				ModificationStore.FULL);
    }

    private PropertyAuditingData GetIdPersistentPropertyAuditingData(Property property) {
        return new PropertyAuditingData(property.Name, property.PropertyAccessorName,
                ModificationStore.FULL, RelationTargetAuditMode.AUDITED, null, null, false);
    }
}

}
