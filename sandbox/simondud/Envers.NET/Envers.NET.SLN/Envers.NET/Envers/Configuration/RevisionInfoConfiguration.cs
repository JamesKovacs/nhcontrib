using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Configuration.Metadata;
using System.Xml;
using System.Reflection;
using NHibernate.Mapping;
using NHibernate.Envers.RevisionInfo;
using NHibernate.Type;
using NHibernate.SqlTypes;
using NHibernate.Envers.Compatibility;

namespace NHibernate.Envers.Configuration
{
    /**
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class RevisionInfoConfiguration {
        private String revisionInfoEntityName;
        private PropertyData revisionInfoIdData;
        private PropertyData revisionInfoTimestampData;
        private IType revisionInfoTimestampType;

        private String revisionPropType;
        private String revisionPropSqlType;

        public RevisionInfoConfiguration() {
            revisionInfoEntityName = "NHibernate.Envers.DefaultRevisionEntity, Envers.NET";
            revisionInfoIdData = new PropertyData("id", "id", "field", ModificationStore._NULL);
            revisionInfoTimestampData = new PropertyData("RevisionDate", "RevisionDate", "field", ModificationStore._NULL);
            revisionInfoTimestampType = new DateTimeType(); //ORIG: LongType();

            revisionPropType = "integer";
        }

        private XmlDocument generateDefaultRevisionInfoXmlMapping() {
            XmlDocument document = new XmlDocument();//ORIG: DocumentHelper.createDocument();

            XmlElement class_mapping = MetadataTools.CreateEntity(document, new AuditTableData(null, null, null, null), null);

            class_mapping.SetAttribute("name", revisionInfoEntityName);
            class_mapping.SetAttribute("table", "REVINFO");

            XmlElement idProperty = MetadataTools.AddNativelyGeneratedId(document,class_mapping, revisionInfoIdData.Name,
                    revisionPropType);
            //ORIG: MetadataTools.addColumn(idProperty, "REV", -1, 0, 0, null);
            XmlElement col = idProperty.OwnerDocument.CreateElement("column");
            col.SetAttribute("name", "REV");
            //idProperty should have a "generator" node otherwise sth. is wrong.
            idProperty.InsertBefore(col, idProperty.GetElementsByTagName("generator")[0]);

            XmlElement timestampProperty = MetadataTools.AddProperty(class_mapping, revisionInfoTimestampData.Name,
                    revisionInfoTimestampType.Name, true, false);
            MetadataTools.AddColumn(timestampProperty, "REVTSTMP", -1, 0, 0, SqlTypeFactory.DateTime.ToString());

            return document;
        }

        private XmlElement generateRevisionInfoRelationMapping() {
            XmlDocument document = new XmlDocument();
            XmlElement rev_rel_mapping = document.CreateElement("key-many-to-one");
            rev_rel_mapping.SetAttribute("type", revisionPropType);
            rev_rel_mapping.SetAttribute("class", revisionInfoEntityName);

            if (revisionPropSqlType != null) {
                // Putting a fake name to make Hibernate happy. It will be replaced later anyway.
                MetadataTools.AddColumn(rev_rel_mapping, "*" , -1, 0, 0, revisionPropSqlType);
            }

            return rev_rel_mapping;
        }

        private void searchForRevisionInfoCfgInProperties(System.Type t, ref bool revisionNumberFound, 
                        ref bool revisionTimestampFound, String accessType) {
           
            foreach (PropertyInfo property in t.GetProperties()) {
                //RevisionNumber revisionNumber = property.getAnnotation(RevisionNumber.class);
                RevisionNumberAttribute revisionNumber = (RevisionNumberAttribute)Attribute.GetCustomAttribute(property, typeof(RevisionNumberAttribute));
                RevisionTimestampAttribute revisionTimestamp = (RevisionTimestampAttribute)Attribute.GetCustomAttribute(property, typeof(RevisionTimestampAttribute));

                if (revisionNumber != null) {
                    if (revisionNumberFound) {
                        throw new MappingException("Only one property may have the attribute [RevisionNumber]!");
                    }

                    System.Type revNrType = property.PropertyType;
                    if (revNrType.Equals( typeof(int)) || revNrType.Equals( typeof(Int16)) || revNrType.Equals( typeof(Int32)) || revNrType.Equals( typeof(Int64))) {
                        revisionInfoIdData = new PropertyData(property.Name, property.Name, accessType, ModificationStore._NULL);
                        revisionNumberFound = true;
                    } else if (revNrType.Equals(typeof(long))) {
                        revisionInfoIdData = new PropertyData(property.Name, property.Name, accessType, ModificationStore._NULL);
                        revisionNumberFound = true;

                        // The default is integer
                        revisionPropType = "long";
                    } else {
                        throw new MappingException("The field decorated with [RevisionNumberAttribute] must be of type " +
                                "int, Int16, Int32, Int64 or long");
                    }

                    // Getting the @Column definition of the revision number property, to later use that info to
                    // generate the same mapping for the relation from an audit table's revision number to the
                    // revision entity revision number.
                    ColumnAttribute revisionPropColumn = (ColumnAttribute)Attribute.GetCustomAttribute(property,typeof(ColumnAttribute));
                    if (revisionPropColumn != null) {
                        revisionPropSqlType = revisionPropColumn.columnDefinition;
                    }
                }

                if (revisionTimestamp != null) {
                    if (revisionTimestampFound) {
                        throw new MappingException("Only one property may be decorated with [RevisionTimestampAttribute]!");
                    }

                    System.Type revisionTimestampType = property.GetType();
                    if (typeof(DateTime).Equals(revisionTimestampType)) {
                        revisionInfoTimestampData = new PropertyData(property.Name, property.Name, accessType, ModificationStore._NULL);
                        revisionTimestampFound = true;
                    } else {
                        throw new MappingException("The field decorated with @RevisionTimestamp must be of type DateTime");
                    }
                }
            }
        }

        private void searchForRevisionInfoCfg(System.Type t, ref bool revisionNumberFound, ref bool revisionTimestampFound) {
            System.Type superT = t.BaseType;
            if (!typeof(System.Object).Equals(t)) {
                searchForRevisionInfoCfg(superT, ref revisionNumberFound, ref revisionTimestampFound);
            }

            searchForRevisionInfoCfgInProperties(t, ref revisionNumberFound, ref revisionTimestampFound,
                    "field");
            searchForRevisionInfoCfgInProperties(t, ref revisionNumberFound, ref revisionTimestampFound,
                    "property");
        }

        //@SuppressWarnings({"unchecked"})
        public RevisionInfoConfigurationResult configure(NHibernate.Cfg.Configuration cfg) {
            ICollection<PersistentClass> classes = cfg.ClassMappings;
            bool revisionEntityFound = false;
            IRevisionInfoGenerator revisionInfoGenerator = null;

            System.Type revisionInfoClass = null;

            foreach (PersistentClass pc in classes) {
                System.Type clazz;
                try {
                    clazz = System.Type.GetType(pc.ClassName, true);
                } catch (System.Exception e) {
                    throw new MappingException(e);
                }

                RevisionEntityAttribute revisionEntity =  (RevisionEntityAttribute)Attribute.GetCustomAttribute( clazz, typeof(RevisionEntityAttribute));
                if (revisionEntity != null) {
                    if (revisionEntityFound) {
                        throw new MappingException("Only one entity may be decorated with [RevisionEntity]!");
                    }

                    // Checking if custom revision entity isn't audited
                    if (Attribute.GetCustomAttribute( clazz, typeof(AuditedAttribute)) != null) {
                        throw new MappingException("An entity decorated with [RevisionEntity] cannot be audited!");
                    }

                    revisionEntityFound = true;

                    bool revisionNumberFound = false;
                    bool revisionTimestampFound = false;

                    searchForRevisionInfoCfg(clazz, ref revisionNumberFound, ref revisionTimestampFound);

                    if (!revisionNumberFound) {
                        throw new MappingException("An entity decorated with [RevisionEntity] must have a field decorated " +
                                "with [RevisionNumber]!");
                    }

                    if (!revisionTimestampFound) {
                        throw new MappingException("An entity decorated with [RevisionEntity] must have a field decorated " +
                                "with [RevisionTimestamp]!");
                    }

                    revisionInfoEntityName = pc.EntityName;

                    revisionInfoClass = pc.MappedClass;
                    revisionInfoTimestampType = pc.GetProperty(revisionInfoTimestampData.Name).Type;
                    revisionInfoGenerator = new DefaultRevisionInfoGenerator(revisionInfoEntityName, revisionInfoClass,
                        /*revisionEntity.value, */revisionInfoTimestampData/*, isTimestampAsDate()*/);
                }
            }

            // In case of a custom revision info generator, the mapping will be null.
            XmlDocument revisionInfoXmlMapping = null;

            if (revisionInfoGenerator == null) {
                revisionInfoClass = typeof(DefaultRevisionEntity);
                revisionInfoGenerator = new DefaultRevisionInfoGenerator(revisionInfoEntityName, revisionInfoClass,
                        revisionInfoTimestampData);
                revisionInfoXmlMapping = generateDefaultRevisionInfoXmlMapping();
            }

            return new RevisionInfoConfigurationResult(
                    revisionInfoGenerator, revisionInfoXmlMapping,
                    new RevisionInfoQueryCreator(revisionInfoEntityName, revisionInfoIdData.Name,
                            revisionInfoTimestampData.Name),
                    generateRevisionInfoRelationMapping(),
                    new RevisionInfoNumberReader(revisionInfoClass, revisionInfoIdData), revisionInfoEntityName);
        }
        
        private bool isTimestampAsDate() {
    	    String typename = revisionInfoTimestampType.Name;
            return "date".Equals(typename) || "time".Equals(typename) || "RevisionDate".Equals(typename);
        }
    }

    public class RevisionInfoConfigurationResult {
        public IRevisionInfoGenerator RevisionInfoGenerator { get; private set; }
        public XmlDocument RevisionInfoXmlMapping { get; private set; }
        public RevisionInfoQueryCreator RevisionInfoQueryCreator { get; private set; }
        public XmlElement RevisionInfoRelationMapping { get; private set; }
        public RevisionInfoNumberReader RevisionInfoNumberReader { get; private set; }
        public String RevisionInfoEntityName { get; private set; }

        public RevisionInfoConfigurationResult(IRevisionInfoGenerator revisionInfoGenerator,
                                        XmlDocument revisionInfoXmlMapping, 
                                        RevisionInfoQueryCreator revisionInfoQueryCreator,
                                        XmlElement revisionInfoRelationMapping,
                                        RevisionInfoNumberReader revisionInfoNumberReader, 
                                        String revisionInfoEntityName) {
            this.RevisionInfoGenerator = revisionInfoGenerator;
            this.RevisionInfoXmlMapping = revisionInfoXmlMapping;
            this.RevisionInfoQueryCreator = revisionInfoQueryCreator;
            this.RevisionInfoRelationMapping = revisionInfoRelationMapping;
            this.RevisionInfoNumberReader = revisionInfoNumberReader;
            this.RevisionInfoEntityName = revisionInfoEntityName;
        }
    }
}
