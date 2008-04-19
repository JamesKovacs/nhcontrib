namespace NHibernate.Burrow.AppBlock.Pagination {
    public abstract class AbstractPaginableRowsCounterQuery<T> : AbstractPaginableQuery<T>, IRowsCounter {
        private QueryRowsCounter dqrc;

        #region IRowsCounter Members

        /// <summary>
        /// Get the row count.
        /// </summary>
        /// <param name="session">The <see cref="ISession"/>.</param>
        /// <returns>The row count.</returns>
        public long GetRowsCount(ISession session) {
            if (dqrc == null)
                dqrc = new QueryRowsCounter(GetRowCountQuery());
            return dqrc.GetRowsCount(session);
        }

        #endregion

        protected abstract IDetachedQuery GetRowCountQuery();
    }
}