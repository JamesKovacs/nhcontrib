using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Envers.Net.Repository;
using NHibernate.Envers;
using Spring.Data.NHibernate.Support;
using Spring.Transaction.Interceptor;
using Envers.Net.Model;

namespace Envers.Net.Repository
{
    
    public class Repository<T> : HibernateDaoSupport, IRepository<T>
    {
        public Repository()
        {
            if (!typeof(T).IsSubclassOf(typeof(DomainObject)))
                throw new NotSupportedException("Generic type should be subclass of DomainObject!");            
        }

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

        public IList GetAllRevisionIds(DomainObject entity)
        {
            IAuditReader auditReader = AuditReaderFactory.Get(Session);
            return auditReader.GetRevisions(entity.GetType(), entity.id);
        }

        public T GetRevision(System.Type tip, long Id, long VersionId)
        {
            IAuditReader auditReader = AuditReaderFactory.Get(Session);
            return auditReader.Find<T>(tip, Id, VersionId);
        }

        #endregion
    }
}
