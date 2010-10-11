using System;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Id;
using NHibernate.Envers.Query;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Tools;
using NHibernate.Envers.Tools.Query;

namespace NHibernate.Envers.Entities.Mapper.Relation.Query
{

    /**
     * Selects data from an audit entity.
     * @author Simon Duduica, port of omonyme class by Adam Warski (adam at warski dot org)
     */
    public sealed class OneAuditEntityQueryGenerator: IRelationQueryGenerator {
        private readonly String queryString;
        private readonly MiddleIdData referencingIdData;

        public OneAuditEntityQueryGenerator(GlobalConfiguration globalCfg, AuditEntitiesConfiguration verEntCfg,
                                               MiddleIdData referencingIdData, String referencedEntityName,
                                               IIdMapper referencedIdMapper) {
            this.referencingIdData = referencingIdData;

            /*
             * The query that we need to create:
             *   SELECT new list(e) FROM versionsReferencedEntity e
             *   WHERE
             * (only entities referenced by the association; id_ref_ing = id of the referencing entity)
             *     e.id_ref_ing = :id_ref_ing AND
             * (selecting e entities at revision :revision)
             *     e.revision = (SELECT max(e2.revision) FROM versionsReferencedEntity e2
             *       WHERE e2.revision <= :revision AND e2.id = e.id) AND
             * (only non-deleted entities)
             *     e.revision_type != DEL
             */
            String revisionPropertyPath = verEntCfg.RevisionNumberPath;
            String originalIdPropertyName = verEntCfg.OriginalIdPropName;

            String versionsReferencedEntityName = verEntCfg.GetAuditEntityName(referencedEntityName);

            // SELECT new list(e) FROM versionsEntity e
            QueryBuilder qb = new QueryBuilder(versionsReferencedEntityName, "e");
            qb.AddProjection("new list", "e", false, false);
            // WHERE
            Parameters rootParameters = qb.RootParameters;
            // e.id_ref_ed = :id_ref_ed
            referencingIdData.PrefixedMapper.AddNamedIdEqualsToQuery(rootParameters, null, true);

            // SELECT max(e.revision) FROM versionsReferencedEntity e2
            QueryBuilder maxERevQb = qb.NewSubQueryBuilder(versionsReferencedEntityName, "e2");
            maxERevQb.AddProjection("max", revisionPropertyPath, false);
            // WHERE
            Parameters maxERevQbParameters = maxERevQb.RootParameters;
            // e2.revision <= :revision
            maxERevQbParameters.AddWhereWithNamedParam(revisionPropertyPath, "<=", "revision");
            // e2.id = e.id
            referencedIdMapper.AddIdsEqualToQuery(maxERevQbParameters,
                    "e." + originalIdPropertyName, "e2." + originalIdPropertyName);

            // e.revision = (SELECT max(...) ...)
            rootParameters.AddWhere(revisionPropertyPath, false, globalCfg.getCorrelatedSubqueryOperator(), maxERevQb);

            // e.revision_type != DEL
            rootParameters.AddWhereWithNamedParam(verEntCfg.RevisionTypePropName, false, "!=", "delrevisiontype");

            StringBuilder sb = new StringBuilder();
            qb.Build(sb, EmptyDictionary<String, object>.Instance);
            queryString = sb.ToString();
        }

        public IQuery GetQuery(IAuditReaderImplementor versionsReader, Object primaryKey, long revision) {
            IQuery query = versionsReader.Session.CreateQuery(queryString);
            query.SetParameter("revision", revision);
            query.SetParameter("delrevisiontype", RevisionType.DEL);
            foreach (QueryParameterData paramData in referencingIdData.PrefixedMapper.MapToQueryParametersFromId(primaryKey)) {
                paramData.SetParameterValue(query);
            }

            return query;
        }
    }
}
