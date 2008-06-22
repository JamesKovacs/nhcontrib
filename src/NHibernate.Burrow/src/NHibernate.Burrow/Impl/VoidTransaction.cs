namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// a transaction manager that does no transaction management
    /// </summary>
    /// <remarks>
    /// this is for in manual transaction mode, so that client can control the transaction itself.
    /// </remarks>
    internal class VoidTransaction : ITransaction
    {
        #region ITransaction Members

        public void Begin(ISession sess)
        {
            return;
        }

        public void Commit()
        {
            return;
        }

        public void Rollback()
        {
            return;
        }

        /// <summary>
        /// whether a transaction has begun
        /// </summary>
        public bool InTransaction
        {
            get { return false; }
        }

        #endregion
    }
}