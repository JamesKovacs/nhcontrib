using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// OBSOLETE replaced by DomainSessionFactoryBase.
    /// </summary>
    [Obsolete]
    public abstract class DomainLayerFactoryBase : IDomainSessionFactory {
        #region IDomainSessionFactory Members

        /// <summary>
        /// this method is for implementing the IDomainSessionFactory
        /// </summary>
        /// <returns></returns>
        IDomainSession IDomainSessionFactory.Create() {
            return (IDomainSession) Create();
        }

        #endregion

        /// <summary>
        /// Create the DomainSession instance    
        /// </summary>
        /// <returns></returns>
        /// <remarks>this is the only method in the base that needs to be overriden</remarks>
        public abstract DomainLayerBase Create();
    }
}