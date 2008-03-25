namespace NHibernate.Burrow.Util.DomainSession
{
    /// <summary>
    /// Loader for getting the DLContainer
    /// </summary>
    public static class DomainSessionContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IDomainSessionContainer Impl
        {
            get { return WebAppAutoDomainSessionContainer.Current; }
        }

        public static IDomainSession DomainSession
        {
            get { return Impl.DomainSession; }
            set { Impl.DomainSession = value; }
        }
    }
}