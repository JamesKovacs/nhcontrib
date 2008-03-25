namespace NHibernate.Burrow.Util.DomainSession
{
    /// <summary>
    /// Factory of domainLayer<see cref="IDomainSession"/>
    /// </summary>
    public interface IDomainSessionFactory
    {
        /// <summary>
        /// Create the domainLayer
        /// </summary>
        /// <returns>IDomainSession that is created</returns>
        IDomainSession Create();
    }
}