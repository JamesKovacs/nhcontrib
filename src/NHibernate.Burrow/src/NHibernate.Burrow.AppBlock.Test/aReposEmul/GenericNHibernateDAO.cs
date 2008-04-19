using System.Collections.Generic;
using NHibernate.Burrow.AppBlock.GenericImpl;
using NHibernate.Burrow.AppBlock.Pagination;
using NHibernate.Criterion;

namespace NHibernate.Burrow.AppBlock.Test.aReposEmul {
    // This implementation is similar to the implementation of 
    // NHibernate in Action book.
    // In your own implementation, you don't need a constructor because you can use a
    // static class like "NHibernateHelper" that provide you the session.
    // Ours tests really don't need this class and it is present only to have a more closer example
    // to your reality.
    // Surfing on the NET you can find many others examples of GenericNHibernateDAO implementation;
    // Provide one is not the target of NHibernate.Burrow.AppBlock. Make your choice like you want.
    public class GenericNHibernateDAO<T, TId> {
        private readonly TestCase workingTest;

        private ISession session;

        public GenericNHibernateDAO(TestCase workingTest) {
            this.workingTest = workingTest;
        }

        public ISession Session {
            get {
                if (session == null)
                    session = workingTest.LastOpenedSession; // NHibernateHelper.GetCurrentSession();
                return session;
            }
            set { session = value; }
        }

        public T FindById(TId id) {
            return Session.Load<T>(id);
        }

        public T FindByIdAndLock(TId id) {
            return Session.Load<T>(id, LockMode.Upgrade);
        }

        public IList<T> FindAll() {
            return Session.CreateCriteria(typeof (T)).List<T>();
        }

        public T MakePersistent(T entity) {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        public void MakeTransient(T entity) {
            Session.Delete(entity);
        }

        public IList<T> Find(DetachedCriteria criteria) {
            return criteria.GetExecutableCriteria(Session).List<T>();
        }

        public IList<T> Find(IDetachedQuery query) {
            return query.GetExecutableQuery(Session).List<T>();
        }

        public T FindUnique(DetachedCriteria criteria) {
            return criteria.GetExecutableCriteria(Session).UniqueResult<T>();
        }

        public T FindUnique(IDetachedQuery query) {
            return query.GetExecutableQuery(Session).UniqueResult<T>();
        }

        // Override this method to provide a PaginableRowCount instance
        // if you are using the SVN-Trunk you have a CriteriaToRowCount transformer
        public virtual IPaginable<T> GetPaginable(DetachedCriteria criteria) {
            return new PaginableCriteria<T>(Session, criteria);
        }

        // Override this method to check the type of 'query' param to provide some type of
        // PaginableRowCount.
        // for example:
        // -secure if query is the implementation of DetachedDynQuery you can use PaginableDynQuery
        // -some times if the query is an implementation of DetachedQuery (see PaginableRowsCounterQuery for instance)
        public virtual IPaginable<T> GetPaginable(IDetachedQuery query) {
            return new PaginableQuery<T>(Session, query);
        }
    }
}