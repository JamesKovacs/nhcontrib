using NHibernate.Burrow.DataContainers;

namespace NHibernate.Burrow.Util.DomainSession {
    /// <summary>
    /// use System.Web.HttpApplication as the container, while this class works as a mediator
    /// </summary>
    public class WebAppAutoDomainSessionContainer : IDomainSessionContainer {
        private const string DL_STORENAME = "HACDomainLayer";

        private static readonly WebAppAutoDomainSessionContainer instance = new WebAppAutoDomainSessionContainer();
        private static readonly object lockObj2 = new object();
        private bool isDisposing = false;

        private WebAppAutoDomainSessionContainer() {}

        private static IDomainSession dlOutOfSession {
            get {
                return
                    AssemblyDataContainer.GetThreadStaticData<IDomainSession>("dlOutOfSession",
                                                                              typeof (WebAppAutoDomainSessionContainer));
            }
            set {
                AssemblyDataContainer.SetThreadStaticData("dlOutOfSession", value,
                                                          typeof (WebAppAutoDomainSessionContainer));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static WebAppAutoDomainSessionContainer Current {
            get { return instance; }
        }

        #region IDomainSessionContainer Members

        /// <summary>
        /// first try to load the dl from the httpsession, if session is out, than return a threadStatic
        /// DomainSession
        /// </summary>
        public IDomainSession DomainSession {
            get {
                lock (lockObj2) {
                    IDomainSession retVal = AssemblyDataContainer.GetHttpSessionData<IDomainSession>(DL_STORENAME);
                    if (retVal == null)
                        return dlOutOfSession;
                    return retVal;
                }
            }
            set {
                lock (lockObj2) {
                    if (!AssemblyDataContainer.SetHttpSessionData(DL_STORENAME, value))
                        dlOutOfSession = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            if (isDisposing) return;
            isDisposing = true;
            if (DomainSession != null)
                DomainSession.Dispose();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        ~WebAppAutoDomainSessionContainer() {
            Dispose();
        }
    }
}