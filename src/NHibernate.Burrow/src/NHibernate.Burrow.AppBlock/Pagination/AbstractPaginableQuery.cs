using System;
using System.Collections.Generic;
using NHibernate.Engine;

namespace NHibernate.Burrow.AppBlock.Pagination {
    public abstract class AbstractPaginableQuery<T> : IPaginable<T> {
        protected abstract IDetachedQuery DetachedQuery { get; }

        #region IPaginable<T> Members

        public abstract ISession GetSession();

        public IList<T> ListAll() {
            ResetPagination(DetachedQuery);
            return InternalExecute(DetachedQuery);
        }

        public IList<T> GetPage(int pageSize, int pageNumber) {
            SetPagination(DetachedQuery, pageSize, pageNumber);
            return InternalExecute(DetachedQuery);
        }

        #endregion

        private IList<T> InternalExecute(IDetachedQuery query) {
            return query.GetExecutableQuery(GetSession()).List<T>();
        }

        private static void ResetPagination(IDetachedQuery query) {
            if (query == null)
                throw new ArgumentNullException("query");

            query.SetFirstResult(default(int)).SetMaxResults(RowSelection.NoValue);
        }

        private static void SetPagination(IDetachedQuery query, int pageSize, int pageNumber) {
            if (query == null)
                throw new ArgumentNullException("query");
            query.SetFirstResult(pageSize*(pageNumber < 1 ? 0 : pageNumber - 1)).SetMaxResults(pageSize);
        }
    }
}