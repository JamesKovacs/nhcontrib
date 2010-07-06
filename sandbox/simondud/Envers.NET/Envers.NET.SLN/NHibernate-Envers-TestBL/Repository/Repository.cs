using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Envers.Net.Repository;
using NHibernate.Envers;
using Spring.Data.NHibernate.Support;
using Spring.Transaction.Interceptor;

namespace Envers.Net.Repository
{
    
    public class Repository<T> : HibernateDaoSupport, IRepository<T>
    {

        #region IRepository<T> Members

        [Transaction]
        public void Add(T entity)
        {
            HibernateTemplate.Save(entity);
        }

        [Transaction]
        public void Update(T entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
        }

        [Transaction]
        public void Remove(T entity)
        {
            HibernateTemplate.Delete(entity);
        }

        [Transaction]
        public T GetById(object entityId)
        {
            return (T)HibernateTemplate.Get(typeof(T), entityId);
        }

        #endregion

        #region IRepository<T> Members


        public ICollection<T> GetByType(string type)
        {
            throw new NotImplementedException();
        }

        public IList<long> GetAllRevisionIds(System.Type tip)
        {
            IAuditReader auditReader = AuditReaderFactory.get(Session);
            return auditReader.getRevisions(entity.getClass(), entity.getId());
        }

        public IList<long> GetRevision(System.Type tip, long Id, long VersionId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
