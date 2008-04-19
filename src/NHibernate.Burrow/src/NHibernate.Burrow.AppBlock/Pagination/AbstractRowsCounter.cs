using System;
using NHibernate.Impl;

namespace NHibernate.Burrow.AppBlock.Pagination {
    /// <summary>
    /// 
    /// </summary>
    public class AbstractRowsCounter : IRowsCounter {
        protected IDetachedQuery dq;

        #region IRowsCounter Members

        /// <summary>
        /// Get the row count.
        /// </summary>
        /// <param name="session">The <see cref="ISession"/>.</param>
        /// <returns>The row count.</returns>
        public long GetRowsCount(ISession session) {
            IQuery q = dq.GetExecutableQuery(session);
            try {
                return (long) q.UniqueResult();
            }
            catch (Exception e) {
                throw new HibernateException(string.Format("Invalid RowsCounter query:{0}", q.QueryString), e);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public IRowsCounter CopyParametersFrom(DetachedQuery origin) {
            origin.SetParametersTo(dq);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public IRowsCounter CopyParametersFrom(DetachedNamedQuery origin) {
            origin.SetParametersTo(dq);
            return this;
        }
    }
}