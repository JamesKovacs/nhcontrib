namespace NHibernate.Burrow.Impl
{
    internal interface ITransaction
    {
        /// <summary>
        /// 
        /// </summary>
        void Begin(ISession sess );

        void Commit();

        void Rollback();
         
        /// <summary>
        /// whether the transaction has begun
        /// </summary>
        bool InTransaction
        {
            get;
        }
    }
}