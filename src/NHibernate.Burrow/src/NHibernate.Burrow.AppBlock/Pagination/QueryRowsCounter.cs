using System;
using NHibernate.Impl;

namespace NHibernate.Burrow.AppBlock.Pagination
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryRowsCounter : AbstractRowsCounter
    {
        /// <summary>
        /// Create a new instance of <see cref="QueryRowsCounter"/>.
        /// </summary>
        /// <param name="hqlRowsCount">The HQL.</param>
        /// <remarks>
        /// If the query is invalid an exception is throw only when <see cref="IRowsCounter.GetRowsCount(ISession)"/>
        /// is called.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="hqlRowsCount"/> is null or empty.</exception>
        public QueryRowsCounter(string hqlRowsCount)
        {
            if (string.IsNullOrEmpty(hqlRowsCount))
            {
                throw new ArgumentNullException("hqlRowsCount");
            }
            dq = new DetachedQuery(hqlRowsCount);
        }

        /// <summary>
        /// Create a new instance of <see cref="QueryRowsCounter"/>.
        /// </summary>
        /// <param name="queryRowCount">The query.</param>
        /// <remarks>
        /// If the query is invalid an exception is throw only when <see cref="IRowsCounter.GetRowsCount(ISession)"/>
        /// is called.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="queryRowCount"/> is null.</exception>
        public QueryRowsCounter(IDetachedQuery queryRowCount)
        {
            if (queryRowCount == null)
            {
                throw new ArgumentNullException("queryRowCount");
            }

            dq = queryRowCount;
        }

        /// <summary>
        /// Transform an gigen <see cref="DetachedQuery"/> (HQL query) to it's rows count.
        /// </summary>
        /// <param name="query">The given <see cref="DetachedQuery"/>.</param>
        /// <returns>
        /// A <see cref="QueryRowsCounter"/> based on <paramref name="query"/>, with row count, using
        /// same parameters and it's values.
        /// </returns>
        /// <exception cref="HibernateException">When the query don't start with 'from' clause.</exception>
        /// <remarks>
        /// Take care to the query; it can't contain any other clause than "from" and "where".
        /// Set the parameters and it's values, of <paramref name="query"/> befor call this method.
        /// </remarks>
        public static QueryRowsCounter Transforming(DetachedQuery query)
        {
            if (!query.Hql.StartsWith("from", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new HibernateException(
                    string.Format(
                        "Can't trasform the HQL to it's counter, the query must start with 'from' clause:{0}", query.Hql));
            }
            QueryRowsCounter result = new QueryRowsCounter("select count(*) " + query.Hql);
            result.CopyParametersFrom(query);
            return result;
        }
    }
}