using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Query.Criteria;
using NHibernate.Envers.Reader;
using NHibernate.Proxy;

namespace NHibernate.Envers.Query.Impl
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public class RevisionsOfEntityQuery : AbstractAuditQuery {
        private readonly bool selectEntitiesOnly;
        private readonly bool selectDeletedEntities;

        public RevisionsOfEntityQuery(AuditConfiguration verCfg,
                                      IAuditReaderImplementor versionsReader,
                                      System.Type cls, bool selectEntitiesOnly,
                                      bool selectDeletedEntities) 
            :base(verCfg, versionsReader, cls)
        {
            

            this.selectEntitiesOnly = selectEntitiesOnly;
            this.selectDeletedEntities = selectDeletedEntities;
        }

        private long GetRevisionNumber(IDictionary<string, object> versionsEntity) {
            AuditEntitiesConfiguration verEntCfg = verCfg.AuditEntCfg;

            String originalId = verEntCfg.OriginalIdPropName;
            String revisionPropertyName = verEntCfg.RevisionFieldName;

            Object revisionInfoObject = ((IDictionary) versionsEntity[originalId])[revisionPropertyName];

            if (revisionInfoObject is INHibernateProxy) {
                return (long) ((INHibernateProxy) revisionInfoObject).HibernateLazyInitializer.Identifier;
            }
            // Not a proxy - must be read from cache or with a join
            return verCfg.RevisionInfoNumberReader.getRevisionNumber(revisionInfoObject);
        }

        public override IList List()
        {
            AuditEntitiesConfiguration verEntCfg = verCfg.AuditEntCfg;

            /*
            The query that should be executed in the versions table:
            SELECT e (unless another projection is specified) FROM ent_ver e, rev_entity r WHERE
              e.revision_type != DEL (if selectDeletedEntities == false) AND
              e.revision = r.revision AND
              (all specified conditions, transformed, on the "e" entity)
              ORDER BY e.revision ASC (unless another order or projection is specified)
             */
            if (!selectDeletedEntities)
            {
                // e.revision_type != DEL AND
                qb.RootParameters.AddWhereWithParam(verEntCfg.RevisionTypePropName, "<>", RevisionType.DEL.Representation);
            }

            // all specified conditions, transformed
            foreach (var criterion in criterions)
            {
                criterion.AddToQuery(verCfg, entityName, qb, qb.RootParameters);
            }

            if (!hasProjection && !hasOrder)
            {
                var revisionPropertyPath = verEntCfg.RevisionNumberPath;
                qb.AddOrder(revisionPropertyPath, true);
            }

            if (!selectEntitiesOnly)
            {
                qb.AddFrom(verCfg.AuditEntCfg.RevisionInfoEntityFullClassName, "r");
                qb.RootParameters.AddWhere(verCfg.AuditEntCfg.RevisionNumberPath, true, "=", "r.id", false);
            }

            var queryResult = BuildAndExecuteQuery();
            if (hasProjection)
            {
                return queryResult;
            }
            var entities = new ArrayList();
            var revisionTypePropertyName = verEntCfg.RevisionTypePropName;

            foreach (var resultRow in queryResult)
            {
                IDictionary<string, object> versionsEntity;
                Object revisionData;

                if (selectEntitiesOnly)
                {
                    versionsEntity = (IDictionary<string, object>) resultRow;
                    revisionData = null;
                }
                else
                {
                    var arrayResultRow = (Object[]) resultRow;
                    versionsEntity = (IDictionary<string, object>) arrayResultRow[0];
                    revisionData = arrayResultRow[1];
                }

                var revision = GetRevisionNumber(versionsEntity);

                var entity = entityInstantiator.CreateInstanceFromVersionsEntity(entityName, versionsEntity, revision);

                entities.Add(selectEntitiesOnly
                                 ? entity
                                 : new[] {entity, revisionData, versionsEntity[revisionTypePropertyName]});
            }

            return entities;
        }
    }
}
