using System;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
    internal class FrameworkEnvironment : IFrameworkEnvironment {
        private static readonly FrameworkEnvironment instance = new FrameworkEnvironment();
        private NHibernateBurrowCfgSection cfg;
        private LocalSafe<IConversation> currentConversationHolder ;

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
        public IConversation CurrentConversation {
            get {
                CheckRunning();
                return currentConversationHolder.Value;
            }
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
            currentConversationHolder = null;
        }

        public bool IsRunning {
            get { return currentConversationHolder != null; }
        }

        public IBurrowConfig Configuration {
            get { return cfg; }
        }

        public void Start() {
            if (IsRunning)
                throw new GeneralException("Burrow Environment is already running");
            currentConversationHolder = new LocalSafe<IConversation>();
        }

        /// <summary>
        /// Gets the num of Spanning Conversations
        /// </summary>
        public int SpanningConversations
        {
            get { return ConversationPool.Instance.ConversationsInPool; }
        }
       

        #endregion

        /// <summary>
        /// Retreive a conversation by <paramref name="id"/> and make it the <see cref="CurrentConversation"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the conversation is successfully retrieved, otherwise false. 
        /// </returns> 
        public void RetrieveCurrent(Guid id) {
            CheckRunning();
            SetCurrentConversation(ConversationPool.Instance[id]);
        }

        private void SetCurrentConversation(IConversation c) {
          
            c.Closed += new EventHandler(conversation_Closed);
            currentConversationHolder.Value = c;
            return;
        }

        private void conversation_Closed(object sender, EventArgs e) {
            currentConversationHolder.Value = null;
        }

        /// <summary>
        /// Start a new conversation
        /// </summary>
        public void StartNewConversation() {
            CheckRunning();
            SetCurrentConversation(ConversationImpl.StartNew());
        }

        private void CheckRunning() {
            if (!IsRunning)
                throw new FrameworkAlreadyShutDownException();
        }
    }
}