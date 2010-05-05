using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Entities
{
    /**
     * Configuration of the user entities: property mapping of the entities, relations, inheritance.
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class EntitiesConfigurations
    {
        private IDictionary<String, EntityConfiguration> entitiesConfigurations;
        private IDictionary<String, EntityConfiguration> notAuditedEntitiesConfigurations;

        // Map versions entity name -> entity name
        private IDictionary<String, String> _entityNamesForVersionsEntityNames = new Dictionary<String, String>();

        public EntitiesConfigurations(IDictionary<String, EntityConfiguration> entitiesConfigurations,
                                      IDictionary<String, EntityConfiguration> notAuditedEntitiesConfigurations)
        {
            this.entitiesConfigurations = entitiesConfigurations;
            this.notAuditedEntitiesConfigurations = notAuditedEntitiesConfigurations;

            GenerateBidirectionRelationInfo();
            GenerateVersionsEntityToEntityNames();
        }

        private void GenerateVersionsEntityToEntityNames() {
        _entityNamesForVersionsEntityNames = new Dictionary<String, String>();

        foreach (String entityName in entitiesConfigurations.Keys) {
            _entityNamesForVersionsEntityNames.Add(entitiesConfigurations[entityName].VersionsEntityName,
                    entityName);
        }
    }

        private void GenerateBidirectionRelationInfo() {
        // Checking each relation if it is bidirectional. If so, storing that information.
        foreach (String entityName in entitiesConfigurations.Keys) {
            EntityConfiguration entCfg = entitiesConfigurations[entityName];
            // Iterating over all relations from that entity
            foreach (RelationDescription relDesc in entCfg.GetRelationsIterator()) {
                // If this is an "owned" relation, checking the related entity, if it has a relation that has
                // a mapped-by attribute to the currently checked. If so, this is a bidirectional relation.
                if (relDesc.getRelationType() == RelationType.TO_ONE ||
						relDesc.getRelationType() == RelationType.TO_MANY_MIDDLE) {
					EntityConfiguration entityConfiguration = entitiesConfigurations[relDesc.getToEntityName()];
					if (entityConfiguration != null) {
						foreach (RelationDescription other in entityConfiguration.GetRelationsIterator()) {
							if (relDesc.getFromPropertyName().Equals(other.getMappedByPropertyName()) &&
									(entityName.Equals(other.getToEntityName()))) {
								relDesc.Bidirectional = true;
								other.Bidirectional = true;
							}
						}
					}
                }
            }
        }
    }

        public EntityConfiguration this [String entityName]
        {
            get { return entitiesConfigurations.ContainsKey(entityName)? entitiesConfigurations[entityName]:null; }
        }

        public EntityConfiguration GetNotVersionEntityConfiguration(String entityName)
        {
            return notAuditedEntitiesConfigurations.ContainsKey(entityName)? notAuditedEntitiesConfigurations[entityName]:null;
        }

        public String GetEntityNameForVersionsEntityName(String versionsEntityName)
        {
            return _entityNamesForVersionsEntityNames.ContainsKey(versionsEntityName) ? _entityNamesForVersionsEntityNames[versionsEntityName] : null;
        }

        public bool IsVersioned(String entityName)
        {
            return entitiesConfigurations.Keys.Contains(entityName);
        }

        public RelationDescription GetRelationDescription(String entityName, String propertyName)
        {
            EntityConfiguration entCfg = entitiesConfigurations[entityName];
            RelationDescription relDesc = entCfg.GetRelationDescription(propertyName);
            if (relDesc != null)
            {
                return relDesc;
            }
            else if (entCfg.ParentEntityName != null)
            {
                // The field may be declared in a superclass ...
                return GetRelationDescription(entCfg.ParentEntityName, propertyName);
            }
            else
            {
                return null;
            }
        }

    }
}
