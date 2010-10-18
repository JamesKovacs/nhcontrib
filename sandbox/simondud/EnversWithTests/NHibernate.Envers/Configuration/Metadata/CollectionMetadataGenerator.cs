using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Envers.Configuration.Metadata.Reader;
using NHibernate.Envers.Entities.Mapper.Relation.Component;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Tools;
using NHibernate.Type;
using NHibernate.Mapping;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Entities.Mapper.Id;
using System.Xml;
using NHibernate.Envers.Entities.Mapper.Relation;
using C5;

namespace NHibernate.Envers.Configuration.Metadata
{
    /// <summary>
    ///Generates metadata for a collection-valued property.
    ///@author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
    ///TODO Simon - implement methods that throw NotImpl....
    /// </summary>
    /// TODO Simon.
    public sealed class CollectionMetadataGenerator {
        private static ILog log = LogManager.GetLogger(typeof(CollectionMetadataGenerator));

        private readonly AuditMetadataGenerator mainGenerator;
        private readonly String propertyName;
        private readonly NHibernate.Mapping.Collection propertyValue;
        private readonly ICompositeMapperBuilder currentMapper;
        private readonly String referencingEntityName;
        private readonly EntityXmlMappingData xmlMappingData;
        private readonly PropertyAuditingData propertyAuditingData;

        private readonly EntityConfiguration referencingEntityConfiguration;
        /**
         * Null if this collection isn't a relation to another entity.
         */
        private readonly String referencedEntityName;

	    /**
         * @param mainGenerator Main generator, giving access to configuration and the basic mapper.
         * @param propertyValue Value of the collection, as mapped by Hibernate.
         * @param currentMapper Mapper, to which the appropriate {@link org.hibernate.envers.entities.mapper.PropertyMapper}
         * will be added.
         * @param referencingEntityName Name of the entity that owns this collection.
         * @param xmlMappingData In case this collection requires a middle table, additional mapping documents will
         * be created using this object.
         * @param propertyAuditingData Property auditing (meta-)data. Among other things, holds the name of the
         * property that references the collection in the referencing entity, the user data for middle (join)
         * table and the value of the <code>@MapKey</code> annotation, if there was one.
         */
        public CollectionMetadataGenerator(AuditMetadataGenerator mainGenerator,
                                           Mapping.Collection propertyValue, ICompositeMapperBuilder currentMapper,
                                           String referencingEntityName, EntityXmlMappingData xmlMappingData,
                                           PropertyAuditingData propertyAuditingData) {
            this.mainGenerator = mainGenerator;
            this.propertyValue = propertyValue;
            this.currentMapper = currentMapper;
            this.referencingEntityName = referencingEntityName;
            this.xmlMappingData = xmlMappingData;
            this.propertyAuditingData = propertyAuditingData;

            this.propertyName = propertyAuditingData.Name;

            referencingEntityConfiguration = mainGenerator.EntitiesConfigurations[referencingEntityName];
            if (referencingEntityConfiguration == null) {
                throw new MappingException("Unable to read auditing configuration for " + referencingEntityName + "!");
            }

            referencedEntityName = MappingTools.getReferencedEntityName(propertyValue.Element);
        }

        public void AddCollection() {
            IType type = propertyValue.Type;

            bool oneToManyAttachedType = type is BagType || type is SetType || type is MapType || type is ListType;
            bool inverseOneToMany = (propertyValue.Element is OneToMany) && (propertyValue.IsInverse);
            bool fakeOneToManyBidirectional = (propertyValue.Element is OneToMany) && (propertyAuditingData.AuditMappedBy != null);

            if (oneToManyAttachedType && (inverseOneToMany || fakeOneToManyBidirectional)) {
                // A one-to-many relation mapped using @ManyToOne and @OneToMany(mappedBy="...")
                AddOneToManyAttached(fakeOneToManyBidirectional);
            } else {
                // All other kinds of relations require a middle (join) table.
                AddWithMiddleTable();
            }
        }

        private MiddleIdData CreateMiddleIdData(IdMappingData idMappingData, String prefix, String entityName) {
            return new MiddleIdData(mainGenerator.VerEntCfg, idMappingData, prefix, entityName,
                    mainGenerator.EntitiesConfigurations.ContainsKey(entityName));
        }

        private void AddOneToManyAttached(bool fakeOneToManyBidirectional) {
            //throw new NotImplementedException();

            log.Debug("Adding audit mapping for property " + referencingEntityName + "." + propertyName +
                    ": one-to-many collection, using a join column on the referenced entity.");

            String mappedBy = GetMappedBy(propertyValue);

            IdMappingData referencedIdMapping = mainGenerator.GetReferencedIdMappingData(referencingEntityName,
                        referencedEntityName, propertyAuditingData, false);
            IdMappingData referencingIdMapping = referencingEntityConfiguration.IdMappingData;

            // Generating the id mappers data for the referencing side of the relation.
            MiddleIdData referencingIdData = CreateMiddleIdData(referencingIdMapping,
                    mappedBy + "_", referencingEntityName);

            // And for the referenced side. The prefixed mapper won't be used (as this collection isn't persisted
            // in a join table, so the prefix value is arbitrary).
            MiddleIdData referencedIdData = CreateMiddleIdData(referencedIdMapping,
                    null, referencedEntityName);

            // Generating the element mapping.
            MiddleComponentData elementComponentData = new MiddleComponentData(
                    new MiddleRelatedComponentMapper(referencedIdData), 0);

            // Generating the index mapping, if an index exists. It can only exists in case a javax.persistence.MapKey
            // annotation is present on the entity. So the middleEntityXml will be not be used. The queryGeneratorBuilder
            // will only be checked for nullnes.
            MiddleComponentData indexComponentData = AddIndex(null, null);

            // Generating the query generator - it should read directly from the related entity.
            IRelationQueryGenerator queryGenerator = new OneAuditEntityQueryGenerator(mainGenerator.GlobalCfg,
                    mainGenerator.VerEntCfg, referencingIdData, referencedEntityName,
                    referencedIdMapping.IdMapper);

            // Creating common mapper data.
            CommonCollectionMapperData commonCollectionMapperData = new CommonCollectionMapperData(
                    mainGenerator.VerEntCfg, referencedEntityName,
                    propertyAuditingData.getPropertyData(),
                    referencingIdData, queryGenerator);

            IPropertyMapper fakeBidirectionalRelationMapper;
            IPropertyMapper fakeBidirectionalRelationIndexMapper;
            if (fakeOneToManyBidirectional)
            {
                // In case of a fake many-to-one bidirectional relation, we have to generate a mapper which maps
                // the mapped-by property name to the id of the related entity (which is the owner of the collection).
                String auditMappedBy = propertyAuditingData.AuditMappedBy;

                // Creating a prefixed relation mapper.
                IIdMapper relMapper = referencingIdMapping.IdMapper.PrefixMappedProperties(
                        MappingTools.createToOneRelationPrefix(auditMappedBy));

                fakeBidirectionalRelationMapper = new ToOneIdMapper(
                        relMapper,
                    // The mapper will only be used to map from entity to map, so no need to provide other details
                    // when constructing the PropertyData.
                        new PropertyData(auditMappedBy, null, null, ModificationStore._NULL),
                        referencedEntityName, false);

                // Checking if there's an index defined. If so, adding a mapper for it.
                if (propertyAuditingData.PositionMappedBy != null)
                {
                    String positionMappedBy = propertyAuditingData.PositionMappedBy;
                    fakeBidirectionalRelationIndexMapper = new SinglePropertyMapper(new PropertyData(positionMappedBy, null, null, ModificationStore._NULL));

                    // Also, overwriting the index component data to properly read the index.
                    indexComponentData = new MiddleComponentData(new MiddleStraightComponentMapper(positionMappedBy), 0);
                }
                else
                {
                    fakeBidirectionalRelationIndexMapper = null;
                }
            }
            else
            {
                fakeBidirectionalRelationMapper = null;
                fakeBidirectionalRelationIndexMapper = null;
            }

            // Checking the type of the collection and adding an appropriate mapper.
            AddMapper(commonCollectionMapperData, elementComponentData, indexComponentData);

            // Storing information about this relation.
            referencingEntityConfiguration.AddToManyNotOwningRelation(propertyName, mappedBy,
                    referencedEntityName, referencingIdData.PrefixedMapper, fakeBidirectionalRelationMapper,
                    fakeBidirectionalRelationIndexMapper);
        }

        /**
         * Adds mapping of the id of a related entity to the given xml mapping, prefixing the id with the given prefix.
         * @param xmlMapping Mapping, to which to add the xml.
         * @param prefix Prefix for the names of properties which will be prepended to properties that form the id.
         * @param columnNameIterator Iterator over the column names that will be used for properties that form the id.
         * @param relatedIdMapping Id mapping data of the related entity.
         */
        private void AddRelatedToXmlMapping(XmlElement xmlMapping, String prefix,
                                            MetadataTools.ColumnNameEnumerator columnNameIterator,
                                            IdMappingData relatedIdMapping) {
            XmlElement properties = (XmlElement) relatedIdMapping.XmlRelationMapping.Clone();
            MetadataTools.PrefixNamesInPropertyElement(properties, prefix, columnNameIterator, true, true);
            foreach (XmlNode idProperty in properties.ChildNodes)
            {
                var tempNode = xmlMapping.OwnerDocument.ImportNode(idProperty, true);
                xmlMapping.AppendChild(tempNode);
            }
        }

        private String GetMiddleTableName(Mapping.Collection value, String entityName) {
            // We check how Hibernate maps the collection.
            if (value.Element is OneToMany && !value.IsInverse) {
                // This must be a @JoinColumn+@OneToMany mapping. Generating the table name, as Hibernate doesn't use a
                // middle table for mapping this relation.
                String refEntName = MappingTools.getReferencedEntityName(value.Element);
                return entityName.Substring(entityName.LastIndexOf(".") + 1) + "_" + 
                    refEntName.Substring(refEntName.LastIndexOf(".") + 1);
            } else {
                // Hibernate uses a middle table for mapping this relation, so we get it's name directly.
                return value.CollectionTable.Name;
            }
        }

        private void AddWithMiddleTable() {
            log.Debug("Adding audit mapping for property " + referencingEntityName + "." + propertyName +
                    ": collection with a join table.");

            // Generating the name of the middle table
            String auditMiddleTableName;
            String auditMiddleEntityName;
            if (!String.IsNullOrEmpty(propertyAuditingData.JoinTable.Name))
            {
                auditMiddleTableName = propertyAuditingData.JoinTable.Name;
                auditMiddleEntityName = propertyAuditingData.JoinTable.Name;
            }
            else
            {
                String middleTableName = GetMiddleTableName(propertyValue, referencingEntityName);
                auditMiddleTableName = mainGenerator.VerEntCfg.GetAuditTableName(null, middleTableName);
                auditMiddleEntityName = mainGenerator.VerEntCfg.GetAuditEntityName(middleTableName);
            }

            log.Debug("Using join table name: " + auditMiddleTableName);

            // Generating the XML mapping for the middle entity, only if the relation isn't inverse.
            // If the relation is inverse, will be later checked by comparing middleEntityXml with null.
            XmlElement middleEntityXml;
            if (!propertyValue.IsInverse)
            {
                // Generating a unique middle entity name
                auditMiddleEntityName = mainGenerator.AuditEntityNameRegister.createUnique(auditMiddleEntityName);

                // Registering the generated name
                mainGenerator.AuditEntityNameRegister.register(auditMiddleEntityName);

                middleEntityXml = CreateMiddleEntityXml(auditMiddleTableName, auditMiddleEntityName, propertyValue.Where);
            }
            else
            {
                middleEntityXml = null;
            }

            // ******
            // Generating the mapping for the referencing entity (it must be an entity).
            // ******
            // Getting the id-mapping data of the referencing entity (the entity that "owns" this collection).
            IdMappingData referencingIdMapping = referencingEntityConfiguration.IdMappingData;

            // Only valid for an inverse relation; null otherwise.
            String mappedBy;

            // The referencing prefix is always for a related entity. So it has always the "_" at the end added.
            String referencingPrefixRelated;
            String referencedPrefix;

            if (propertyValue.IsInverse)
            {
                // If the relation is inverse, then referencedEntityName is not null.
                mappedBy = GetMappedBy(propertyValue.CollectionTable, mainGenerator.Cfg.GetClassMapping(referencedEntityName));

                referencingPrefixRelated = mappedBy + "_";
                referencedPrefix = StringTools.GetLastComponent(referencedEntityName);
            }
            else
            {
                mappedBy = null;

                referencingPrefixRelated = StringTools.GetLastComponent(referencingEntityName) + "_";
                referencedPrefix = referencedEntityName == null ? "element" : propertyName;
            }

            // Storing the id data of the referencing entity: original mapper, prefixed mapper and entity name.
            MiddleIdData referencingIdData = CreateMiddleIdData(referencingIdMapping,
                    referencingPrefixRelated, referencingEntityName);

            // Creating a query generator builder, to which additional id data will be added, in case this collection
            // references some entities (either from the element or index). At the end, this will be used to build
            // a query generator to read the raw data collection from the middle table.
            QueryGeneratorBuilder queryGeneratorBuilder = new QueryGeneratorBuilder(mainGenerator.GlobalCfg,
                    mainGenerator.VerEntCfg, referencingIdData, auditMiddleEntityName);

            // Adding the XML mapping for the referencing entity, if the relation isn't inverse.
            if (middleEntityXml != null)
            {
                // Adding related-entity (in this case: the referencing's entity id) id mapping to the xml.
                AddRelatedToXmlMapping(middleEntityXml, referencingPrefixRelated,
                        MetadataTools.GetColumnNameEnumerator(propertyValue.Key.ColumnIterator.GetEnumerator()),
                        referencingIdMapping);
            }

            // ******
            // Generating the element mapping.
            // ******
            MiddleComponentData elementComponentData = AddValueToMiddleTable(propertyValue.Element, middleEntityXml,
                    queryGeneratorBuilder, referencedPrefix, propertyAuditingData.JoinTable.InverseJoinColumns);

            // ******
            // Generating the index mapping, if an index exists.
            // ******
            MiddleComponentData indexComponentData = AddIndex(middleEntityXml, queryGeneratorBuilder);

            // ******
            // Generating the property mapper.
            // ******
            // Building the query generator.
            IRelationQueryGenerator queryGenerator = queryGeneratorBuilder.Build(new Collection<MiddleComponentData>{elementComponentData, indexComponentData});

            // Creating common data
            CommonCollectionMapperData commonCollectionMapperData = new CommonCollectionMapperData(
                    mainGenerator.VerEntCfg, auditMiddleEntityName,
                    propertyAuditingData.getPropertyData(),
                    referencingIdData, queryGenerator);

            // Checking the type of the collection and adding an appropriate mapper.
            AddMapper(commonCollectionMapperData, elementComponentData, indexComponentData);

            // ******
            // Storing information about this relation.
            // ******
            StoreMiddleEntityRelationInformation(mappedBy);
        }

        private MiddleComponentData AddIndex(XmlElement middleEntityXml, QueryGeneratorBuilder queryGeneratorBuilder) {

            if (propertyValue is IndexedCollection)
            {
                IndexedCollection indexedValue = (IndexedCollection)propertyValue;
                String mapKey = propertyAuditingData.MapKey;
                if (mapKey == null)
                {
                    // This entity doesn't specify a javax.persistence.MapKey. Mapping it to the middle entity.
                    return AddValueToMiddleTable(indexedValue.Index, middleEntityXml,
                            queryGeneratorBuilder, "mapkey", null);
                }
                else
                {
                    IdMappingData referencedIdMapping = 
                            mainGenerator.EntitiesConfigurations[referencedEntityName].IdMappingData;
                    int currentIndex = queryGeneratorBuilder == null ? 0 : queryGeneratorBuilder.CurrentIndex;
                    if ("".Equals(mapKey))
                    {
                        // The key of the map is the id of the entity.
                        return new MiddleComponentData(new MiddleMapKeyIdComponentMapper(mainGenerator.VerEntCfg,
                                referencedIdMapping.IdMapper), currentIndex);
                    }
                    else
                    {
                        // The key of the map is a property of the entity.
                        return new MiddleComponentData(new MiddleMapKeyPropertyComponentMapper(mapKey,
                                propertyAuditingData.AccessType), currentIndex);
                    }
                }
            }
            else
            {
                // No index - creating a dummy mapper.
                return new MiddleComponentData(new MiddleDummyComponentMapper(), 0);
            }
        }

        /**
         *
         * @param value Value, which should be mapped to the middle-table, either as a relation to another entity,
         * or as a simple value.
         * @param xmlMapping If not <code>null</code>, xml mapping for this value is added to this element.
         * @param queryGeneratorBuilder In case <code>value</code> is a relation to another entity, information about it
         * should be added to the given.
         * @param prefix Prefix for proeprty names of related entities identifiers.
         * @param joinColumns Names of columns to use in the xml mapping, if this array isn't null and has any elements.
         * @return Data for mapping this component.
         */
        //@SuppressWarnings({"unchecked"})
        private MiddleComponentData AddValueToMiddleTable(IValue value, XmlElement xmlMapping,
                                                          QueryGeneratorBuilder queryGeneratorBuilder,
                                                          String prefix, JoinColumnAttribute[] joinColumns) {
            IType type = value.Type;
            if (type is ManyToOneType) {
                String prefixRelated = prefix + "_";

                String referencedEntityName = MappingTools.getReferencedEntityName(value);

                IdMappingData referencedIdMapping = mainGenerator.GetReferencedIdMappingData(referencingEntityName,
                        referencedEntityName, propertyAuditingData, true);

                // Adding related-entity (in this case: the referenced entities id) id mapping to the xml only if the
                // relation isn't inverse (so when <code>xmlMapping</code> is not null).
                if (xmlMapping != null) {
                    AddRelatedToXmlMapping(xmlMapping, prefixRelated,
                            joinColumns != null && joinColumns.Length > 0
                                    ? MetadataTools.GetColumnNameEnumerator(joinColumns)
                                    : MetadataTools.GetColumnNameEnumerator(value.ColumnIterator.GetEnumerator()),
                            referencedIdMapping);
                }

                // Storing the id data of the referenced entity: original mapper, prefixed mapper and entity name.
                MiddleIdData referencedIdData = CreateMiddleIdData(referencedIdMapping,
                        prefixRelated, referencedEntityName);
                // And adding it to the generator builder.
                queryGeneratorBuilder.AddRelation(referencedIdData);

                return new MiddleComponentData(new MiddleRelatedComponentMapper(referencedIdData),
                        queryGeneratorBuilder.CurrentIndex);
            } else {
                // Last but one parameter: collection components are always insertable
                bool mapped = mainGenerator.BasicMetadataGenerator.AddBasic(xmlMapping,
                        new PropertyAuditingData(prefix, "field", ModificationStore.FULL, RelationTargetAuditMode.AUDITED, null, null, false),
                        value, null, true, true);

                if (mapped) {
                    // Simple values are always stored in the first item of the array returned by the query generator.
                    return new MiddleComponentData(new MiddleSimpleComponentMapper(mainGenerator.VerEntCfg, prefix), 0);
                } else {
                    mainGenerator.ThrowUnsupportedTypeException(type, referencingEntityName, propertyName);
                    // Impossible to get here.
                    throw new AssertionFailure();
                }
            }
        }

        private void AddMapper(CommonCollectionMapperData commonCollectionMapperData, MiddleComponentData elementComponentData,
                               MiddleComponentData indexComponentData) {
            IType type = propertyValue.Type;
            if (type is SortedSetType) {
                currentMapper.AddComposite(propertyAuditingData.getPropertyData(),
                        new BasicCollectionMapper<IDictionary>(commonCollectionMapperData,
                        typeof(TreeSet<>), typeof(SortedSetProxy<>), elementComponentData));
            } 
            else if (type is SetType) {
                currentMapper.AddComposite(propertyAuditingData.getPropertyData(),
                        new BasicCollectionMapper<Set>(commonCollectionMapperData,
                        typeof(HashedSet<>), typeof(SetProxy<>), elementComponentData));
            } 
            else
                throw new NotImplementedException();
            //else if (type is SortedMapType) {
            //    // Indexed collection, so <code>indexComponentData</code> is not null.
            //    currentMapper.addComposite(propertyAuditingData.getPropertyData(),
            //            new MapCollectionMapper<Map>(commonCollectionMapperData,
            //            TreeMap.class, SortedMapProxy.class, elementComponentData, indexComponentData));
            //} else if (type is MapType) {
            //    // Indexed collection, so <code>indexComponentData</code> is not null.
            //    currentMapper.addComposite(propertyAuditingData.getPropertyData(),
            //            new MapCollectionMapper<Map>(commonCollectionMapperData,
            //            HashMap.class, MapProxy.class, elementComponentData, indexComponentData));
            //} else if (type is BagType) {
            //    currentMapper.addComposite(propertyAuditingData.getPropertyData(),
            //            new BasicCollectionMapper<List>(commonCollectionMapperData,
            //            ArrayList.class, ListProxy.class, elementComponentData));
            //} else if (type is ListType) {
            //    // Indexed collection, so <code>indexComponentData</code> is not null.
            //    currentMapper.addComposite(propertyAuditingData.getPropertyData(),
            //            new ListCollectionMapper(commonCollectionMapperData,
            //            elementComponentData, indexComponentData));
            //} else {
            //    mainGenerator.ThrowUnsupportedTypeException(type, referencingEntityName, propertyName);
            //}
        }

        private void StoreMiddleEntityRelationInformation(String mappedBy) {
            // Only if this is a relation (when there is a referenced entity).
            if (referencedEntityName != null)
            {
                if (propertyValue.IsInverse)
                {
                    referencingEntityConfiguration.AddToManyMiddleNotOwningRelation(propertyName, mappedBy, referencedEntityName);
                }
                else
                {
                    referencingEntityConfiguration.addToManyMiddleRelation(propertyName, referencedEntityName);
                }
            }
        }

        private XmlElement CreateMiddleEntityXml(String auditMiddleTableName, String auditMiddleEntityName, String where) {
            String schema = mainGenerator.GetSchema(propertyAuditingData.JoinTable.Schema, propertyValue.CollectionTable);
            String catalog = mainGenerator.GetCatalog(propertyAuditingData.JoinTable.Catalog, propertyValue.CollectionTable);

            XmlElement middleEntityXml = MetadataTools.CreateEntity(xmlMappingData.newAdditionalMapping(),
                    new AuditTableData(auditMiddleEntityName, auditMiddleTableName, schema, catalog), null);
            XmlElement middleEntityXmlId = middleEntityXml.OwnerDocument.CreateElement("composite-id");
            middleEntityXml.AppendChild(middleEntityXmlId);

            // If there is a where clause on the relation, adding it to the middle entity.
            if (where != null)
            {
                middleEntityXml.SetAttribute("where", where);
            }

            middleEntityXmlId.SetAttribute("name", mainGenerator.VerEntCfg.OriginalIdPropName);

            // Adding the revision number as a foreign key to the revision info entity to the composite id of the
            // middle table.
            mainGenerator.AddRevisionInfoRelation(middleEntityXmlId);

            // Adding the revision type property to the entity xml.
            mainGenerator.AddRevisionType(middleEntityXml);

            // All other properties should also be part of the primary key of the middle entity.
            return middleEntityXmlId;
        }

        private String GetMappedBy(Mapping.Collection collectionValue) {
            PersistentClass referencedClass = ((OneToMany)collectionValue.Element).AssociatedClass;

            // If there's an @AuditMappedBy specified, returning it directly.
            String auditMappedBy = propertyAuditingData.AuditMappedBy;
            if (auditMappedBy != null)
            {
                return auditMappedBy;
            }

            IEnumerator<Property> assocClassProps = referencedClass.PropertyIterator.GetEnumerator();

            while (assocClassProps.MoveNext())
            {
                Property property = assocClassProps.Current;

                if (Toolz.IteratorsContentEqual(property.Value.ColumnIterator.GetEnumerator(),
                        collectionValue.Key.ColumnIterator.GetEnumerator()))
                {
                    return property.Name;
                }
            }

            throw new MappingException("Unable to read the mapped by attribute for " + propertyName + " in "
                    + referencingEntityName + "!");
        }

        private String GetMappedBy(Table collectionTable, PersistentClass referencedClass) {
            // If there's an @AuditMappedBy specified, returning it directly.
            String auditMappedBy = propertyAuditingData.AuditMappedBy;
            if (auditMappedBy != null)
            {
                return auditMappedBy;
            }

            IEnumerator<Property> properties = referencedClass.PropertyIterator.GetEnumerator();
            while (properties.MoveNext())
            {
                Property property = properties.Current;
                if (property.Value is ICollection)
                {
                    // The equality is intentional. We want to find a collection property with the same collection table.
                    //noinspection ObjectEquality
                    if (((NHibernate.Mapping.Collection)property.Value).CollectionTable == collectionTable)
                    {
                        return property.Name;
                    }
                }
            }

            throw new MappingException("Unable to read the mapped by attribute for " + propertyName + " in "
                    + referencingEntityName + "!");
        }
    }
}
