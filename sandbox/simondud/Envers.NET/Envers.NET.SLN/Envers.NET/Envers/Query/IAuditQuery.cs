using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NHibernate.Envers.Query.Order;
using NHibernate.Envers.Query.Criteria;
using NHibernate.Envers.Query.Projection;

namespace NHibernate.Envers.Query
{
    /**
     * @author Adam Warski (adam at warski dot org)
     * @see org.hibernate.Criteria
     */
    public interface IAuditQuery {
        IList getResultList();

        Object getSingleResult();

        IAuditQuery Add(IAuditCriterion criterion);

        IAuditQuery AddProjection(IAuditProjection projection);

        IAuditQuery addOrder(IAuditOrder order);

        IAuditQuery setMaxResults(int maxResults);

	    IAuditQuery setFirstResult(int firstResult);

        IAuditQuery setCacheable(bool cacheable);

        IAuditQuery setCacheRegion(String cacheRegion);

        IAuditQuery setComment(String comment);

        IAuditQuery setFlushMode(FlushMode flushMode);

        IAuditQuery setCacheMode(CacheMode cacheMode);

        IAuditQuery setTimeout(int timeout);

        IAuditQuery setLockMode(LockMode lockMode);
    }
}
