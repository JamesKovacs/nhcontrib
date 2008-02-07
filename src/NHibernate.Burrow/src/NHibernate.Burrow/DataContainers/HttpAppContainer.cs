namespace NHibernate.Burrow.DataContainers {
    /// <summary>
    /// use System.Web.HttpApplication as the container, while this class works as a mediator
    /// </summary>
    public class HttpAppContainer : IDLContainer {
        private const string DL_STORENAME = "HACDomainLayer";

        private static readonly HttpAppContainer instance = new HttpAppContainer();
        private static readonly object lockObj2 = new object();
        private bool isDisposing = false;

        private HttpAppContainer() {}

        private static IDomainSession dlOutOfSession {
            get {
                return
                    AssemblyDataContainer.GetThreadStaticData<IDomainSession>("dlOutOfSession", typeof (HttpAppContainer));
            }
            set { AssemblyDataContainer.SetThreadStaticData("dlOutOfSession", value, typeof (HttpAppContainer)); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static HttpAppContainer Current {
            get { return instance; }
        }

        #region IDLContainer Members

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
        ~HttpAppContainer() {
            Dispose();
        }
    }
}