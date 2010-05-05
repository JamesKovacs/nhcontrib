using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Collection;
using NHibernate.Envers.Entities.Mapper;

namespace NHibernate.Envers.Synchronization.Work
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public class PersistentCollectionChangeWorkUnit : AbstractAuditWorkUnit, IAuditWorkUnit {
        private IList<PersistentCollectionChangeData> collectionChanges;
        public String ReferencingPropertyName{get; private set;}

        public PersistentCollectionChangeWorkUnit(ISessionImplementor sessionImplementor, String entityName,
											      AuditConfiguration auditCfg, IPersistentCollection collection,
											      CollectionEntry collectionEntry, Object snapshot, Object id,
                                                  String referencingPropertyName) 
            : base(sessionImplementor, entityName, auditCfg, 
                    new PersistentCollectionChangeWorkUnitId(id, collectionEntry.Role))
        {
		    this.ReferencingPropertyName = referencingPropertyName;

            collectionChanges = auditCfg.getEntCfg()[EntityName].PropertyMapper
                    .MapCollectionChanges(referencingPropertyName, collection, snapshot, id);
        }

        public PersistentCollectionChangeWorkUnit(ISessionImplementor sessionImplementor, String entityName,
                                                  AuditConfiguration verCfg, Object id,
                                                  IList<PersistentCollectionChangeData> collectionChanges,
                                                  String referencingPropertyName) 
            :base(sessionImplementor, entityName, verCfg, id)
        {
            this.collectionChanges = collectionChanges;
            this.ReferencingPropertyName = referencingPropertyName;
        }

        public override bool ContainsWork()
        {
            return collectionChanges != null && collectionChanges.Count != 0;
        }

        public override IDictionary<String, Object> GenerateData(Object revisionData)
        {
            throw new NotSupportedException("Cannot generate data for a collection change work unit!");
        }

        // TODO Simon @SuppressWarnings({"unchecked"})
        public void perform(ISession session, Object revisionData) {
            AuditEntitiesConfiguration entitiesCfg = verCfg.getAuditEntCfg();

            foreach (PersistentCollectionChangeData persistentCollectionChangeData in collectionChanges) {
                // Setting the revision number
                ((IDictionary<String, Object>) persistentCollectionChangeData.getData()[entitiesCfg.OriginalIdPropName])
                        .Add(entitiesCfg.RevisionFieldName, revisionData);

                session.Save(persistentCollectionChangeData.EntityName, persistentCollectionChangeData.getData());
            }
        }

        public IList<PersistentCollectionChangeData> getCollectionChanges() {
            return collectionChanges;
        }

        public override IAuditWorkUnit Merge(AddWorkUnit second)
        {
            return null;
        }

        public override IAuditWorkUnit Merge(ModWorkUnit second)
        {
            return null;
        }

        public override IAuditWorkUnit Merge(DelWorkUnit second)
        {
            return null;
        }

        public override IAuditWorkUnit Merge(CollectionChangeWorkUnit second)
        {
            return null;
        }

        public override IAuditWorkUnit Merge(FakeBidirectionalRelationWorkUnit second)
        {
            return null;
        }

        public override IAuditWorkUnit Dispatch(IWorkUnitMergeVisitor first)
        {
            if (first is PersistentCollectionChangeWorkUnit) {
                PersistentCollectionChangeWorkUnit original = (PersistentCollectionChangeWorkUnit) first;

                // Merging the collection changes in both work units.

                // First building a map from the ids of the collection-entry-entities from the "second" collection changes,
                // to the PCCD objects. That way, we will be later able to check if an "original" collection change
                // should be added, or if it is overshadowed by a new one.
                IDictionary<Object, PersistentCollectionChangeData> newChangesIdMap = new Dictionary<Object, PersistentCollectionChangeData>();
                foreach (PersistentCollectionChangeData persistentCollectionChangeData in getCollectionChanges()) {
                    newChangesIdMap.Add(
                            getOriginalId(persistentCollectionChangeData),
                            persistentCollectionChangeData);
                }

                // This will be the list with the resulting (merged) changes.
                List<PersistentCollectionChangeData> mergedChanges = new List<PersistentCollectionChangeData>();

                // Including only those original changes, which are not overshadowed by new ones.
                foreach (PersistentCollectionChangeData originalCollectionChangeData in original.getCollectionChanges()) {
                    if (!newChangesIdMap.ContainsKey(getOriginalId(originalCollectionChangeData))) {
                        mergedChanges.Add(originalCollectionChangeData);
                    }
                }

                // Finally adding all of the new changes to the end of the list
                mergedChanges = (List<PersistentCollectionChangeData>)mergedChanges.Concat(getCollectionChanges());

                return new PersistentCollectionChangeWorkUnit(sessionImplementor, EntityName, verCfg, EntityId, mergedChanges, 
                        ReferencingPropertyName);
            } else {
                throw new Exception("Trying to merge a " + first + " with a PersitentCollectionChangeWorkUnit. " +
                        "This is not really possible.");
            }
        }

        private Object getOriginalId(PersistentCollectionChangeData persistentCollectionChangeData) {
            return persistentCollectionChangeData.getData()[verCfg.getAuditEntCfg().OriginalIdPropName];
        }

        /**
         * A unique identifier for a collection work unit. Consists of an id of the owning entity and the name of
         * the entity plus the name of the field (the role). This is needed because such collections aren't entities
         * in the "normal" mapping, but they are entities for Envers.
         */
        private class PersistentCollectionChangeWorkUnitId{
            private static readonly long serialVersionUID = -8007831518629167537L;
            
            private readonly Object ownerId;
            private readonly String role;

            public PersistentCollectionChangeWorkUnitId(Object ownerId, String role) {
                this.ownerId = ownerId;
                this.role = role;
            }

            public override bool Equals(Object o) {
                if (this == o) return true;
                if (o == null || GetType() != o.GetType()) return false;

                PersistentCollectionChangeWorkUnitId that = (PersistentCollectionChangeWorkUnitId) o;

                if (ownerId != null ? !ownerId.Equals(that.ownerId) : that.ownerId != null) return false;
                //noinspection RedundantIfStatement
                if (role != null ? !role.Equals(that.role) : that.role != null) return false;

                return true;
            }

            public override int GetHashCode() {
                int result = ownerId != null ? ownerId.GetHashCode() : 0;
                result = 31 * result + (role != null ? role.GetHashCode() : 0);
                return result;
            }
        }
    }
}
