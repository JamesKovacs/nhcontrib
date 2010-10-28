using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NHibernate.Envers.Configuration.Metadata;
using NHibernate.Envers.Tools.Graph;
using NHibernate.Envers.Entities;
using NHibernate.Mapping;
using NHibernate.Envers.Configuration.Metadata.Reader;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Type;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Configuration.Metadata
{
    /// <summary>
    ///@author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
    /// </summary>
    public sealed class AuditMetadataGenerator
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AuditMetadataGenerator));

        public Cfg.Configuration Cfg { get; private set; }
        public GlobalConfiguration GlobalCfg { get; private set; }
        public AuditEntitiesConfiguration VerEntCfg { get; private set; }
        private XmlElement revisionInfoRelationMapping;

        /*
         * Generators for different kinds of property values/types.
         */
        public BasicMetadataGenerator BasicMetadataGenerator { get; private set; }
        private ComponentMetadataGenerator componentMetadataGenerator;
        private IdMetadataGenerator idMetadataGenerator;
        private ToOneRelationMetadataGenerator toOneRelationMetadataGenerator;

        /*
         * Here information about already generated mappings will be accumulated.
         */
        public IDictionary<String, EntityConfiguration> EntitiesConfigurations { get; private set; }
        public IDictionary<String, EntityConfiguration> NotAuditedEntitiesConfigurations { get; private set; }

        public AuditEntityNameRegister AuditEntityNameRegister { get; private set; }
        public ClassesAuditingData ClassesAuditingData { get; private set; }

        // Map entity name -> (join descriptor -> element describing the "versioned" join)
        private IDictionary<String, IDictionary<Join, XmlElement>> entitiesJoins;

        public AuditMetadataGenerator(Cfg.Configuration cfg, GlobalConfiguration globalCfg,
                                      AuditEntitiesConfiguration verEntCfg,
                                      XmlElement revisionInfoRelationMapping,
                                      AuditEntityNameRegister auditEntityNameRegister,
                                      ClassesAuditingData classesAuditingData)
        {
            this.Cfg = cfg;
            this.GlobalCfg = globalCfg;
            this.VerEntCfg = verEntCfg;
            this.revisionInfoRelationMapping = revisionInfoRelationMapping;

            this.BasicMetadataGenerator = new BasicMetadataGenerator();
            this.componentMetadataGenerator = new ComponentMetadataGenerator(this);
            this.idMetadataGenerator = new IdMetadataGenerator(this);
            this.toOneRelationMetadataGenerator = new ToOneRelationMetadataGenerator(this);

            this.AuditEntityNameRegister = auditEntityNameRegister;
            this.ClassesAuditingData = classesAuditingData;

            EntitiesConfigurations = new Dictionary<String, EntityConfiguration>();
            NotAuditedEntitiesConfigurations = new Dictionary<String, EntityConfiguration>();
            entitiesJoins = new Dictionary<String, IDictionary<Join, XmlElement>>();
        }

        /**
         * Clones the revision info relation mapping, so that it can be added to other mappings. Also, the name of
         * the property and the column are set properly.
         * @return A revision info mapping, which can be added to other mappings (has no parent).
         */
        private XmlElement CloneAndSetupRevisionInfoRelationMapping(XmlDocument doc)
        {
            XmlElement rev_mapping = (XmlElement)doc.ImportNode(revisionInfoRelationMapping, true);
            rev_mapping.SetAttribute("name", VerEntCfg.RevisionFieldName);

            MetadataTools.AddOrModifyColumn(rev_mapping, VerEntCfg.RevisionFieldName);

            return rev_mapping;
        }

        public void AddRevisionInfoRelation(XmlElement any_mapping)
        {
            any_mapping.AppendChild(CloneAndSetupRevisionInfoRelationMapping(any_mapping.OwnerDocument));
        }

        public void AddRevisionType(XmlElement any_mapping)
        {
            var revTypeProperty = MetadataTools.AddProperty(any_mapping, VerEntCfg.RevisionTypePropName,
                    VerEntCfg.RevisionTypePropType, true, false);
            revTypeProperty.SetAttribute("type", typeof(RevisionTypeType).AssemblyQualifiedName);
        }

        //@SuppressWarnings({"unchecked"})
        public void AddValue(XmlElement parent, IValue value, ICompositeMapperBuilder currentMapper, String entityName,
                      EntityXmlMappingData xmlMappingData, PropertyAuditingData propertyAuditingData,
                      bool insertable, bool firstPass)
        {
            IType type = value.Type;

            // only first pass
            if (firstPass)
            {
                if (BasicMetadataGenerator.AddBasic(parent, propertyAuditingData, value, currentMapper,
                        insertable, false))
                {
                    // The property was mapped by the basic generator.
                    return;
                }
            }

            if (type is ComponentType)
            {
                // both passes
                componentMetadataGenerator.AddComponent(parent, propertyAuditingData, value, currentMapper,
                        entityName, xmlMappingData, firstPass);
            }
            else if (type is ManyToOneType)
            {
                // only second pass
                if (!firstPass)
                {
                    toOneRelationMetadataGenerator.AddToOne(parent, propertyAuditingData, value, currentMapper,
                            entityName, insertable);
                }
            }
            else if (type is OneToOneType)
            {
                // only second pass
                if (!firstPass)
                {
                    toOneRelationMetadataGenerator.AddOneToOneNotOwning(propertyAuditingData, value,
                            currentMapper, entityName);
                }
            }
            else if (type is CollectionType)
            {
                // only second pass
                if (!firstPass)
                {
                    CollectionMetadataGenerator collectionMetadataGenerator = new CollectionMetadataGenerator(this,
                            (Mapping.Collection)value, currentMapper, entityName, xmlMappingData,
                            propertyAuditingData);
                    collectionMetadataGenerator.AddCollection();
                }
            }
            else
            {
                if (firstPass)
                {
                    // If we got here in the first pass, it means the basic mapper didn't map it, and none of the
                    // above branches either.
                    ThrowUnsupportedTypeException(type, entityName, propertyAuditingData.Name);
                }
            }
        }

        //@SuppressWarnings({"unchecked"})
        private void AddProperties(XmlElement parent, IEnumerator<Property> properties, ICompositeMapperBuilder currentMapper,
                                   ClassAuditingData auditingData, String entityName, EntityXmlMappingData xmlMappingData,
                                   bool firstPass)
        {
            while (properties.MoveNext())
            {
                Property property = properties.Current;
                String propertyName = property.Name;
                PropertyAuditingData propertyAuditingData = auditingData.getPropertyAuditingData(propertyName);
                if (propertyAuditingData != null)
                {
                    AddValue(parent, property.Value, currentMapper, entityName, xmlMappingData, propertyAuditingData,
                            property.IsInsertable, firstPass);
                }
            }
        }

        private bool CheckPropertiesAudited(IEnumerator<Property> properties, ClassAuditingData auditingData)
        {
            while (properties.MoveNext())
            {
                Property property = properties.Current;
                String propertyName = property.Name;
                PropertyAuditingData propertyAuditingData = auditingData.getPropertyAuditingData(propertyName);
                if (propertyAuditingData == null)
                {
                    return false;
                }
            }

            return true;
        }

        public String GetSchema(String schemaFromAnnotation, Table table)
        {
            // Get the schema from the annotation ...
            String schema = schemaFromAnnotation;
            // ... if empty, try using the default ...
            if (String.IsNullOrEmpty(schema))
            {
                schema = GlobalCfg.getDefaultSchemaName();

                // ... if still empty, use the same as the normal table.
                if (String.IsNullOrEmpty(schema))
                {
                    schema = table.Schema;
                }
            }
            return schema;
        }

        public String GetCatalog(String catalogFromAnnotation, Table table)
        {
            // Get the catalog from the annotation ...
            String catalog = catalogFromAnnotation;
            // ... if empty, try using the default ...
            if (string.IsNullOrEmpty(catalog))
            {
                catalog = GlobalCfg.getDefaultCatalogName();

                // ... if still empty, use the same as the normal table.
                if (String.IsNullOrEmpty(catalog))
                {
                    catalog = table.Catalog;
                }
            }
            return catalog;
        }

        //@SuppressWarnings({"unchecked"})
        private void CreateJoins(PersistentClass pc, XmlElement parent, ClassAuditingData auditingData)
        {
            IEnumerator<Join> joins = pc.JoinIterator.GetEnumerator();

            IDictionary<Join, XmlElement> JoinElements = new Dictionary<Join, XmlElement>();
            entitiesJoins.Add(pc.EntityName, JoinElements);

            while (joins.MoveNext())
            {
                Join join = joins.Current;

                // Checking if all of the join properties are audited
                if (!CheckPropertiesAudited(join.PropertyIterator.GetEnumerator(), auditingData))
                {
                    continue;
                }

                // Determining the table name. If there is no entry in the dictionary, just constructing the table name
                // as if it was an entity (by appending/prepending configured strings).
                String originalTableName = join.Table.Name;
                String auditTableName = auditingData.SecondaryTableDictionary[originalTableName];
                if (auditTableName == null)
                {
                    auditTableName = VerEntCfg.GetAuditEntityName(originalTableName);
                }

                String schema = GetSchema(auditingData.AuditTable.schema, join.Table);
                String catalog = GetCatalog(auditingData.AuditTable.catalog, join.Table);

                XmlElement joinElement = MetadataTools.CreateJoin(parent, auditTableName, schema, catalog);
                JoinElements.Add(join, joinElement);

                XmlElement joinKey = joinElement.OwnerDocument.CreateElement("key");
                joinElement.AppendChild(joinKey);
                MetadataTools.AddColumns(joinKey, (IEnumerator<ISelectable>)join.Key.ColumnIterator.GetEnumerator());
                MetadataTools.AddColumn(joinKey, VerEntCfg.RevisionFieldName, -1, 0, 0, null);
            }
        }

        //@SuppressWarnings({"unchecked"})
        private void AddJoins(PersistentClass pc, ICompositeMapperBuilder currentMapper, ClassAuditingData auditingData,
                              String entityName, EntityXmlMappingData xmlMappingData, bool firstPass)
        {
            IEnumerator<Join> joins = pc.JoinIterator.GetEnumerator();

            while (joins.MoveNext())
            {
                Join join = joins.Current;
                XmlElement joinElement = entitiesJoins[entityName][join];

                if (joinElement != null)
                {
                    AddProperties(joinElement, (IEnumerator<Property>)join.PropertyIterator.GetEnumerator(), currentMapper, auditingData, entityName,
                            xmlMappingData, firstPass);
                }
            }
        }

        private void AddSingleInheritancePersisterHack(XmlElement class_mapping)
        {
            class_mapping.SetAttribute("persister", "NHibernate.Envers.Entity.EnversSingleTableEntityPersister");
        }

        private void AddJoinedInheritancePersisterHack(XmlElement class_mapping)
        {
            class_mapping.SetAttribute("persister", "NHibernate.Envers.Entity.EnversJoinedSubclassEntityPersister");
        }

        private void AddTablePerClassInheritancePersisterHack(XmlElement class_mapping)
        {
            class_mapping.SetAttribute("persister", "NHibernate.Envers.Entity.EnversUnionSubclassEntityPersister");
        }

        //@SuppressWarnings({"unchecked"})
        private Triple<XmlElement, IExtendedPropertyMapper, String> GenerateMappingData(
                PersistentClass pc, EntityXmlMappingData xmlMappingData, AuditTableData auditTableData,
                IdMappingData idMapper)
        {
            bool hasDiscriminator = pc.Discriminator != null;

            XmlElement class_mapping = MetadataTools.CreateEntity(xmlMappingData.MainXmlMapping, auditTableData,
                    hasDiscriminator ? pc.DiscriminatorValue : null);
            IExtendedPropertyMapper propertyMapper = new MultiPropertyMapper();

            // Checking if there is a discriminator column
            if (hasDiscriminator)
            {
                XmlElement discriminator_element = class_mapping.OwnerDocument.CreateElement("discriminator");
                class_mapping.AppendChild(discriminator_element);
                MetadataTools.AddColumns(discriminator_element, (IEnumerator<ISelectable>)pc.Discriminator.ColumnIterator.GetEnumerator());
                discriminator_element.SetAttribute("type", pc.Discriminator.Type.Name);
            }

            InheritanceType.Type parentInheritance = InheritanceType.GetForParent(pc);
            switch (parentInheritance)
            {
                case InheritanceType.Type.NONE:
                    break;

                case InheritanceType.Type.SINGLE:
                    AddSingleInheritancePersisterHack(class_mapping);
                    break;

                case InheritanceType.Type.JOINED:
                    AddJoinedInheritancePersisterHack(class_mapping);
                    break;

                case InheritanceType.Type.TABLE_PER_CLASS:
                    AddTablePerClassInheritancePersisterHack(class_mapping);
                    break;
            }

            // Adding the id mapping
            XmlNode xmlMp = class_mapping.OwnerDocument.ImportNode(idMapper.XmlMapping,true);
            class_mapping.AppendChild(xmlMp);

            // Adding the "revision type" property
            AddRevisionType(class_mapping);

            return Triple<XmlElement, IExtendedPropertyMapper, string>.Make(class_mapping, propertyMapper, null);
        }

        private Triple<XmlElement, IExtendedPropertyMapper, String> GenerateInheritanceMappingData(
                PersistentClass pc, EntityXmlMappingData xmlMappingData, AuditTableData auditTableData,
                String inheritanceMappingType)
        {
            String extendsEntityName = VerEntCfg.GetAuditEntityName(pc.Superclass.EntityName);
            XmlElement class_mapping = MetadataTools.CreateSubclassEntity(xmlMappingData.MainXmlMapping,
                    inheritanceMappingType, auditTableData, extendsEntityName, pc.DiscriminatorValue);

            // The id and revision type is already mapped in the parent

            // Getting the property mapper of the parent - when mapping properties, they need to be included
            String parentEntityName = pc.Superclass.EntityName;

            EntityConfiguration parentConfiguration = EntitiesConfigurations[parentEntityName];
            if (parentConfiguration == null)
            {
                throw new MappingException("Entity '" + pc.EntityName + "' is audited, but its superclass: '" +
                        parentEntityName + "' is not.");
            }

            IExtendedPropertyMapper parentPropertyMapper = parentConfiguration.PropertyMapper;
            IExtendedPropertyMapper propertyMapper = new SubclassPropertyMapper(new MultiPropertyMapper(), parentPropertyMapper);

            return Triple<XmlElement, IExtendedPropertyMapper, String>.Make(class_mapping, propertyMapper, parentEntityName);
        }

        //@SuppressWarnings({"unchecked"})
        public void GenerateFirstPass(PersistentClass pc, ClassAuditingData auditingData,
                                      EntityXmlMappingData xmlMappingData, bool isAudited)
        {
            String schema = GetSchema(auditingData.AuditTable.schema, pc.Table);
            String catalog = GetCatalog(auditingData.AuditTable.catalog, pc.Table);

            String entityName = pc.EntityName;
            if (!isAudited)
            {
                IdMappingData _idMapper = idMetadataGenerator.AddId(pc);

                if (_idMapper == null)
                {
                    // Unsupported id mapping, e.g. key-many-to-one. If the entity is used in auditing, an exception
                    // will be thrown later on.
                    if (log.IsDebugEnabled)
                    {
                        log.Debug("Unable to create auditing id mapping for entity " + entityName +
                            ", because of an unsupported Hibernate id mapping (e.g. key-many-to-one).");
                    }
                    return;
                }

                //ORIG:
                //IExtendedPropertyMapper propertyMapper = null;
                //String parentEntityName = null;
                EntityConfiguration _entityCfg = new EntityConfiguration(entityName, _idMapper, null,
                        null);
                NotAuditedEntitiesConfigurations.Add(entityName, _entityCfg);
                return;
            }

            if (log.IsDebugEnabled)
            {
                log.Debug("Generating first-pass auditing mapping for entity " + entityName + ".");
            }

            String auditEntityName = VerEntCfg.GetAuditEntityName(entityName);
            String auditTableName = VerEntCfg.GetAuditTableName(entityName, pc.Table.Name);

            // Registering the audit entity name, now that it is known
            AuditEntityNameRegister.register(auditEntityName);

            AuditTableData auditTableData = new AuditTableData(auditEntityName, auditTableName, schema, catalog);

            // Generating a mapping for the id
            IdMappingData idMapper = idMetadataGenerator.AddId(pc);

            InheritanceType.Type inheritanceType = InheritanceType.GetForChild(pc);

            // These properties will be read from the mapping data
            XmlElement class_mapping;
            IExtendedPropertyMapper propertyMapper;
            String parentEntityName;

            Triple<XmlElement, IExtendedPropertyMapper, String> mappingData;

            // Reading the mapping data depending on inheritance type (if any)
            switch (inheritanceType)
            {
                case InheritanceType.Type.NONE:
                    mappingData = GenerateMappingData(pc, xmlMappingData, auditTableData, idMapper);
                    break;

                case InheritanceType.Type.SINGLE:
                    mappingData = GenerateInheritanceMappingData(pc, xmlMappingData, auditTableData, "subclass");
                    break;

                case InheritanceType.Type.JOINED:
                    mappingData = GenerateInheritanceMappingData(pc, xmlMappingData, auditTableData, "joined-subclass");

                    AddJoinedInheritancePersisterHack(mappingData.First);

                    // Adding the "key" element with all id columns...
                    XmlElement keyMapping = mappingData.First.OwnerDocument.CreateElement("key");
                    mappingData.First.AppendChild(keyMapping);
                    MetadataTools.AddColumns(keyMapping, (IEnumerator<ISelectable>)pc.Table.PrimaryKey.ColumnIterator.GetEnumerator());

                    // ... and the revision number column, read from the revision info relation mapping.
                    keyMapping.AppendChild((XmlElement)CloneAndSetupRevisionInfoRelationMapping(keyMapping.OwnerDocument).GetElementsByTagName("column")[0].Clone());
                    break;

                case InheritanceType.Type.TABLE_PER_CLASS:
                    mappingData = GenerateInheritanceMappingData(pc, xmlMappingData, auditTableData, "union-subclass");

                    AddTablePerClassInheritancePersisterHack(mappingData.First);

                    break;

                default:
                    throw new AssertionFailure("Envers.NET: AuditMetadataGenerator.GenerateFirstPass: Impossible enum value.");
            }

            class_mapping = mappingData.First;
            propertyMapper = mappingData.Second;
            parentEntityName = mappingData.Third;

            xmlMappingData.ClassMapping = class_mapping;

            // Mapping unjoined properties
            AddProperties(class_mapping, (IEnumerator<Property>)pc.UnjoinedPropertyIterator.GetEnumerator(), propertyMapper,
                    auditingData, pc.EntityName, xmlMappingData,
                    true);

            // Creating and mapping joins (first pass)
            CreateJoins(pc, class_mapping, auditingData);
            AddJoins(pc, propertyMapper, auditingData, pc.EntityName, xmlMappingData, true);

            // Storing the generated configuration
            EntityConfiguration entityCfg = new EntityConfiguration(auditEntityName, idMapper,
                    propertyMapper, parentEntityName);
            EntitiesConfigurations.Add(pc.EntityName, entityCfg);
        }

        public void GenerateSecondPass(PersistentClass pc, ClassAuditingData auditingData,
                                       EntityXmlMappingData xmlMappingData) {
            String entityName = pc.EntityName;
            if (log.IsDebugEnabled)
            {
                log.Debug("Generating first-pass auditing mapping for entity " + entityName + ".");
            }

            ICompositeMapperBuilder propertyMapper = EntitiesConfigurations[entityName].PropertyMapper;

            // Mapping unjoined properties
            XmlElement parent = xmlMappingData.ClassMapping;

            AddProperties(parent, (IEnumerator<Property>) pc.UnjoinedPropertyIterator.GetEnumerator(),
                    propertyMapper, auditingData, entityName, xmlMappingData, false);

            // Mapping joins (second pass)
            AddJoins(pc, propertyMapper, auditingData, entityName, xmlMappingData, false);
        }

        // Getters for generators and configuration

        public void ThrowUnsupportedTypeException(IType type, String entityName, String propertyName)
        {
            String message = "Type not supported for auditing: " + type.Name +
                    ", on entity " + entityName + ", property '" + propertyName + "'.";

            throw new MappingException(message);
        }

        /**
         * Reads the id mapping data of a referenced entity.
         * @param entityName Name of the entity which is the source of the relation.
         * @param referencedEntityName Name of the entity which is the target of the relation.
         * @param propertyAuditingData Auditing data of the property that is the source of the relation.
         * @param allowNotAuditedTarget Are not-audited target entities allowed.
         * @throws MappingException If a relation from an audited to a non-audited entity is detected, which is not
         * mapped using {@link RelationTargetAuditMode#NOT_AUDITED}.
         * @return The id mapping data of the related entity. 
         */
        public IdMappingData GetReferencedIdMappingData(String entityName, String referencedEntityName,
                                                PropertyAuditingData propertyAuditingData,
                                                bool allowNotAuditedTarget)
        {
            EntityConfiguration configuration;
            if (EntitiesConfigurations.Keys.Contains(referencedEntityName))
                configuration = EntitiesConfigurations[referencedEntityName];
            else
            {
                RelationTargetAuditMode relationTargetAuditMode = propertyAuditingData.getRelationTargetAuditMode();

                if (!NotAuditedEntitiesConfigurations.Keys.Contains(referencedEntityName) ||
                    !allowNotAuditedTarget || !RelationTargetAuditMode.NOT_AUDITED.Equals(relationTargetAuditMode))
                {
                    throw new MappingException("An audited relation from " + entityName + "."
                            + propertyAuditingData.Name + " to a not audited entity " + referencedEntityName + "!"
                            + (allowNotAuditedTarget ?
                                " Such mapping is possible, but has to be explicitly defined using [Audited(TargetAuditMode = RelationTargetAuditMode.NOT_AUDITED)]." :
                                ""));
                }
                else configuration = NotAuditedEntitiesConfigurations[referencedEntityName];
            }
            return configuration.IdMappingData;
        }
    }
}

