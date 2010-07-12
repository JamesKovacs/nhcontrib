using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Query.Criteria;
using NHibernate.Envers.Query.Order;
using NHibernate.Envers.Query.Projection;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Query.Impl
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    public abstract class AbstractAuditQuery : IAuditQuery {
        protected EntityInstantiator entityInstantiator;
        protected IList<IAuditCriterion> criterions;

        protected String entityName;
        protected String versionsEntityName;
        protected QueryBuilder qb;

        protected bool hasProjection;
        protected bool hasOrder;

        protected readonly AuditConfiguration verCfg;
        private readonly IAuditReaderImplementor versionsReader;

        protected AbstractAuditQuery(AuditConfiguration verCfg, IAuditReaderImplementor versionsReader,
                                        System.Type cls) {
            this.verCfg = verCfg;
            this.versionsReader = versionsReader;

            criterions = new List<IAuditCriterion>();
            entityInstantiator = new EntityInstantiator(verCfg, versionsReader);

            entityName = cls.Name;
            versionsEntityName = verCfg.AuditEntCfg.GetAuditEntityName(entityName);

            qb = new QueryBuilder(versionsEntityName, "e");
        }

        protected IList BuildAndExecuteQuery() {
            StringBuilder querySb = new StringBuilder();
            IDictionary<String, Object> queryParamValues = new Dictionary<String, Object>();

            qb.Build(querySb, queryParamValues);

            IQuery query = versionsReader.Session.CreateQuery(querySb.ToString());
            foreach (KeyValuePair<String, Object> paramValue in queryParamValues) {
                query.SetParameter(paramValue.Key, paramValue.Value);
            }

            setQueryProperties(query);

            return query.List();
        }

        public abstract IList List{ get;}

        public IList GetResultList(){
            return List;
        }

        public Object GetSingleResult(){
            IList result = List;

            if (result == null || result.Count == 0) {
                //throw new NoResultException();
                throw new HibernateException("No result!");
            }

            if (result.Count > 1) {
                throw new NonUniqueResultException(result.Count);
            }

            return result[0];
        }

        public IAuditQuery Add(IAuditCriterion criterion) {
            criterions.Add(criterion);
            return this;
        }

        // Projection and order

        public IAuditQuery AddProjection(IAuditProjection projection) {
            Triple<String, String, Boolean> projectionData = projection.GetData(verCfg);
            hasProjection = true;
            qb.AddProjection(projectionData.First, projectionData.Second, projectionData.Third);
            return this;
        }

        public IAuditQuery AddOrder(IAuditOrder order) {
            hasOrder = true;

            Pair<String, Boolean> orderData = order.getData(verCfg);
            qb.AddOrder(orderData.First, orderData.Second);
            return this;
        }

        // Query properties

        private int? maxResults;
        private int? firstResult;
        private bool? cacheable;
        private String cacheRegion;
        private String comment;
        private FlushMode flushMode;
        private CacheMode cacheMode;
        private int? timeout;
        private LockMode lockMode;

        public IAuditQuery SetMaxResults(int _maxResults) {
            this.maxResults = _maxResults;
            return this;
        }

        public IAuditQuery SetFirstResult(int firstResult) {
            this.firstResult = firstResult;
            return this;
        }

        public IAuditQuery SetCacheable(bool cacheable) {
            this.cacheable = cacheable;
            return this;
        }

        public IAuditQuery SetCacheRegion(String cacheRegion) {
            this.cacheRegion = cacheRegion;
            return this;
        }

        public IAuditQuery SetComment(String comment) {
            this.comment = comment;
            return this;
        }

        public IAuditQuery SetFlushMode(FlushMode flushMode) {
            this.flushMode = flushMode;
            return this;
        }

        public IAuditQuery SetCacheMode(CacheMode cacheMode) {
            this.cacheMode = cacheMode;
            return this;
        }

        public IAuditQuery SetTimeout(int timeout) {
            this.timeout = timeout;
            return this;
        }

        public IAuditQuery SetLockMode(LockMode lockMode) {
            this.lockMode = lockMode;
            return this;
        }

        protected void setQueryProperties(IQuery query) {
            if (maxResults != null) query.SetMaxResults((int)maxResults);
            if (firstResult != null) query.SetFirstResult((int)firstResult);
            if (cacheable != null) query.SetCacheable((bool)cacheable);
            if (cacheRegion != null) query.SetCacheRegion(cacheRegion);
            if (comment != null) query.SetComment(comment);
            if (flushMode != null) query.SetFlushMode(flushMode);
            if (cacheMode != null) query.SetCacheMode(cacheMode);
            if (timeout != null) query.SetTimeout((int)timeout);
            if (lockMode != null) query.SetLockMode("e", lockMode);
        }
    }
}
