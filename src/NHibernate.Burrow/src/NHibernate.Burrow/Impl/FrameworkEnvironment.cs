using System;
using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
    internal class FrameworkEnvironment : IFrameworkEnvironment {
        private static readonly FrameworkEnvironment instance = new FrameworkEnvironment();
        private NHibernateBurrowCfgSection cfg;
        private LocalSafe<DomainContext> currentDomainContextHolder;

        private FrameworkEnvironment() {
            cfg = NHibernateBurrowCfgSection.CreateInstance();
            Start();
        }

        public static FrameworkEnvironment Instance {
            get { return instance; }
        }

        /// <summary>
        /// The currentConversationHolder context conversation
        /// </summary>
        public DomainContext CurrentDomainContext
        {
            get {
                CheckRunning();
                if(currentDomainContextHolder.Value != null && currentDomainContextHolder.Value.Closed)
                    currentDomainContextHolder.Value = null;
                return currentDomainContextHolder.Value;
            }
            private set {
                currentDomainContextHolder.Value = value;
            }
        }


        /// <summary>
        /// Initialize a Domain Context 
        /// </summary>
        /// <remarks>
        /// Please read the remarks of the <see cref="DomainContext"/>
        /// You normally don't need to call this method
        /// </remarks>
        /// <param name="states">
        /// Initialized the domain context with a collection of states
        /// </param>
        public void StartNewDomainContext(NameValueCollection states)
        {
            if (CurrentDomainContext == null)
                CurrentDomainContext = DomainContext.StartNew(states);
            else
                throw new BurrowException("DomainContext is already initialized");
        }

        #region IFrameworkEnvironment Members

        /// <summary>
        /// ShutDown the whole environment
        /// </summary>
        /// <remarks>
        /// it will roll back every conversation in the pool
        /// </remarks>
        public void ShutDown() {
            CheckRunning();
            if (new Facade().Alive)
                throw new GeneralException("Domain must be closed before ShutDown, call Facade.CloseDomain() first");
            ConversationPool.Instance.Clear();
            currentDomainContextHolder = null;
        }

        public bool IsRunning {
            get { return currentDomainContextHolder != null; }
        }

        public IBurrowConfig Configuration {
            get { return cfg; }
        }

        public void Start() {
            if (IsRunning)
                throw new GeneralException("Burrow Environment is already running");
          
            currentDomainContextHolder = new LocalSafe<DomainContext>();
        }

        /// <summary>
        /// Gets the num of Spanning Conversations
        /// </summary>
        public int SpanningConversations
        {
            get { return ConversationPool.Instance.ConversationsInPool; }
        }
       

        #endregion

 

        private void CheckRunning() {
            if (!IsRunning)
                throw new FrameworkAlreadyShutDownException();
        }
    }
}