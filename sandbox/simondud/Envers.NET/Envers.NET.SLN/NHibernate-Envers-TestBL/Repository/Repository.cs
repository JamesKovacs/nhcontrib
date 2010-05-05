using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Envers.Net.Repository;
using Spring.Data.NHibernate.Support;
using Spring.Transaction.Interceptor;

namespace Envers.Net.Repository
{
    
    public class Repository<Type> : HibernateDaoSupport, IRepository<Type>
    {

        #region IRepository<Type> Members

        [Transaction]
        public void Add(Type entity)
        {
            HibernateTemplate.Save(entity);
        }

        [Transaction]
        public void Update(Type entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
        }

        [Transaction]
        public void Remove(Type entity)
        {
            HibernateTemplate.Delete(entity);
        }

        [Transaction]
        public Type GetById(object entityId)
        {
            return (Type)HibernateTemplate.Get(typeof(Type), entityId);
        }

        #endregion

        #region IRepository<Type> Members


        public ICollection<Type> GetByType(string type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
