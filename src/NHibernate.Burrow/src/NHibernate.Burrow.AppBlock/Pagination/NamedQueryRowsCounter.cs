using System;
using NHibernate.Impl;

namespace NHibernate.Burrow.AppBlock.Pagination {
    /// <summary>
    /// 
    /// </summary>
    public class NamedQueryRowsCounter : AbstractRowsCounter {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryRowsCount"></param>
        public NamedQueryRowsCounter(string queryRowsCount) {
            if (string.IsNullOrEmpty(queryRowsCount))
                throw new ArgumentNullException("queryRowsCount");
            dq = new DetachedNamedQuery(queryRowsCount);
        }

        public NamedQueryRowsCounter(DetachedNamedQuery queryRowsCount) {
            if (queryRowsCount == null)
                throw new ArgumentNullException("queryRowsCount");

            dq = queryRowsCount;
        }
    }
}