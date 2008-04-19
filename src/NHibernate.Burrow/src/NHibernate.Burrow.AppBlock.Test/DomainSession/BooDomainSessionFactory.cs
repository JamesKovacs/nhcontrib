using System.Collections.Generic;
using NHibernate.Burrow.AppBlock.DomainSession;

namespace NHibernate.Burrow.AppBlock.Test.DomainSession {
    public class BooDomainSessionFactory : IDomainSessionFactory {
        #region IDomainSessionFactory Members

        /// <summary>
        /// Create the domainLayer
        /// </summary>
        /// <returns>IDomainSession that is created</returns>
        public IDictionary<string, IDomainSession> Create() {
            IDictionary<string, IDomainSession> retVal = new Dictionary<string, IDomainSession>();
            retVal.Add(typeof (BooDomainSession).Name, new BooDomainSession());
            return retVal;
        }

        #endregion
    }
}