using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace NHibernate.Burrow.Util.DomainSession {
    /// <summary>
    /// Loader for getting the DLContainer
    /// </summary>
    public class DomainSessionContainer {
        private readonly IDictionary<string, IDomainSession> dss = new Dictionary<string, IDomainSession>();

        private DomainSessionContainer() {
            foreach (KeyValuePair<string, IDomainSession> pair in Factory.Create()) Set(pair.Key, pair.Value);
        }

        public static DomainSessionContainer Instance {
            get { return Nested.DomainSessionContainer; }
        }

        private static IDomainSessionFactory Factory {
            get {
                return (IDomainSessionFactory) Util.Create(
                                                   ConfigurationManager.AppSettings["NHibernate.Burrow.Util.DomainSessionFactory"]);
            }
        }

        public IDomainSession Get(string key) {
            IDomainSession retVal;

            if (IsInWebContext())
                retVal = (IDomainSession) HttpContext.Current.Session[key];
            else
                dss.TryGetValue(key, out retVal);
            return retVal;
        }

        private void Set(string key, IDomainSession val) {
            if (IsInWebContext())
                HttpContext.Current.Session[key] = val;
            else
                dss[key] = val;
        }

        private static bool IsInWebContext() {
            return HttpContext.Current != null;
        }

        #region Nested type: Nested

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested {
            internal static readonly DomainSessionContainer DomainSessionContainer =
                new DomainSessionContainer();

            static Nested() {}
        }

        #endregion
    }
}