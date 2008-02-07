using System;
using log4net;
using NHibernate.Burrow.DataContainers;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// OBSOLETE please use DomainSessionBase instead
    /// </summary>
    /// <remarks>
    /// DomainSession is not thread safe. If you use MHGeneralLib then its lifetime is decided by the MHGeneralLib
    /// Normally the domain Layer has the same lifetime as a HttpSession if the domain is runned under a httpApplication.
    /// So, please avoid store persistent objects in the domainLayer. It's mainly for storing non-entity ojbects.
    /// </remarks>
    [Obsolete]
    public abstract class DomainLayerBase : IDomainSession {
        private object currentUserId;
        private bool isDisposing = false;

        #region static part

        private static readonly object lockobj = new object();


        /// <summary>
        /// Return the current layer from the current Container's DomainSession property
        /// if Container is not used, then it will use a treadStatic instance;
        /// </summary>
        public static DomainLayerBase Current {
            get {
                lock (lockobj) {
                    
                        if (DLContainer.DomainSession == null)
                            DLContainer.DomainSession = GetInstance();
                        return (DomainLayerBase)DLContainer.DomainSession;
                     
                }
            }
        }

        private static DomainLayerBase GetInstance() {
            return  (DomainLayerBase)Configuration.GetDomainSessionFactory().Create();
        }

        /// <summary>
        /// 
        /// </summary>
        ~DomainLayerBase() {
            Dispose();
        }

        #endregion static part

        /// <summary>
        /// before the Domain Layer get closed
        /// </summary>
        public event EventHandler PreClose;

        #region properties

        internal static ISessionManager SessMngr {
            get { return SessionManager.Instance; }
        }

        /// <summary>
        /// Gets and Sets the UserId of the Current User, this is set by Client
        /// </summary>
        public object CurrentUserId {
            get { return currentUserId; }
            set { currentUserId = value; }
        }

        #endregion properties

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public void Dispose() {
            if (isDisposing) return;
            isDisposing = true;
        }

        /// <summary>
        /// Call it when a http requests ends
        /// </summary>
        public void Close() {
            try {
                if (PreClose != null) PreClose(this, new EventArgs());
            }
            finally {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Call it when a http request starts
        /// </summary>
        public virtual void Open() {}

        #endregion public methods
    }
}