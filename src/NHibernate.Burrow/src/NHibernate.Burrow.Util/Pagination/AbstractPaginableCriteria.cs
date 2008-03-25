using System;
using System.Collections.Generic;
using NHibernate.Burrow.Util.NH.Expression;
using NHibernate.Engine;

namespace NHibernate.Burrow.Util.Pagination
{
    public abstract class AbstractPaginableCriteria<T> : IPaginable<T>
    {
        /// <summary>
        /// Take care <see cref="DetachedCriteria"/> is not <see cref="NHibernate.Expression.DetachedCriteria"/>.
        /// The official 1.2.x DetachedCriteria don't ha methods for pagination.
        /// </summary>
        protected abstract DetachedCriteria Criteria { get; }

        #region IPaginable<T> Members

        /// <summary>
        /// Session getter.
        /// </summary>
        /// <returns>The <see cref="ISession"/>.</returns>
        public abstract ISession GetSession();

        /// <summary>
        /// All results without paging.
        /// </summary>
        /// <returns>The list of all instances.</returns>
        public IList<T> ListAll()
        {
            ResetPagination(Criteria);
            return InternalExecute(Criteria);
        }

        /// <summary>
        /// Page result getter.
        /// </summary>
        /// <param name="pageSize">The page's elements quantity.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>The page's elements list.</returns>
        /// <remarks>The max size of the list is <paramref name="pageSize"/>.</remarks>
        public IList<T> GetPage(int pageSize, int pageNumber)
        {
            SetPagination(Criteria, pageSize, pageNumber);
            return InternalExecute(Criteria);
        }

        #endregion

        private IList<T> InternalExecute(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(GetSession()).List<T>();
        }

        private static void ResetPagination(DetachedCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            criteria.SetFirstResult(default(int)).SetMaxResults(RowSelection.NoValue);
        }

        private static void SetPagination(DetachedCriteria criteria, int pageSize, int pageNumber)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }
            criteria.SetFirstResult(pageSize * (pageNumber < 1 ? 0 : pageNumber - 1)).SetMaxResults(pageSize);
        }
    }
}