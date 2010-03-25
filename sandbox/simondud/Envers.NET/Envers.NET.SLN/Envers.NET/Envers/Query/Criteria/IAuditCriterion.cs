using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;

namespace NHibernate.Envers.Query.Criteria
{
    public interface IAuditCriterion
    {
        void addToQuery(AuditConfiguration auditCfg, String entityName, IQueryBuilder qb, IParameters parameters);
    }
}
