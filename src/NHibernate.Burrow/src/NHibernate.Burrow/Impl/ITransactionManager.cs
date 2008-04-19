namespace NHibernate.Burrow.Impl
{
    internal interface ITransactionManager
    {
        /// <summary>
        /// 
        /// </summary>
        void BeginTransaction(ISession sess);

        void CommitTransaction();

        void RollbackTransaction();

        void Dispose();
    }
}