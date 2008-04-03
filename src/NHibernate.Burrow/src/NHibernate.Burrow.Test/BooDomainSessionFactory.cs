using System.Collections.Generic;
using NHibernate.Burrow.Util.DomainSession;

namespace NHibernate.Burrow.Test {
    public class BooDomainSessionFactory : IDomainSessionFactory {
        /// <summary>
        /// Create the domainLayer
        /// </summary>
        /// <returns>IDomainSession that is created</returns>
        public IDictionary<string, IDomainSession> Create() {
            IDictionary<string, IDomainSession> retVal = new Dictionary<string, IDomainSession>();
            retVal.Add(typeof(BooDomainSession).Name, new BooDomainSession());
            return retVal;
        }
    }
}