using System.Collections.Generic;

namespace NHibernate.Burrow.AppBlock.DomainSession
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
        IDictionary<string, IDomainSession> Create();
    }
}