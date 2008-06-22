using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl
{
    internal class FrameworkEnvironment : IFrameworkEnvironment
    {
        private static readonly FrameworkEnvironment instance = new FrameworkEnvironment();
        private NHibernateBurrowCfgSection cfg;
        private LocalSafe<WorkSpace> currentWorkSpaceHolder;

        private FrameworkEnvironment()
        {
            cfg = NHibernateBurrowCfgSection.CreateInstance();
            Start();
        }

        public static FrameworkEnvironment Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// The currentConversationHolder context conversation
        /// </summary>
        public WorkSpace CurrentWorkSpace
        {
            get
            {
                CheckRunning();
                if (currentWorkSpaceHolder.Value != null && currentWorkSpaceHolder.Value.Closed)
                {
                    currentWorkSpaceHolder.Value = null;
                }
                return currentWorkSpaceHolder.Value;
            }
           set
           {
                currentWorkSpaceHolder.Value = value;
           }
        }

        #region IFrameworkEnvironment Members

        /// <summary>
        /// ShutDown the whole environment
        /// </summary>
        /// <remarks>
        /// it will roll back every conversation in the pool
        /// </remarks>
        public void ShutDown()
        {
            CheckRunning();
            if (new BurrowFramework().WorkSpaceIsReady)
            {
                throw new GeneralException(
                    "Domain must be closed before ShutDown, call BurrowFramework.CloseWorkSpace() first");
            }
            ConversationPool.Instance.Clear();
            currentWorkSpaceHolder = null;
        }

        public bool IsRunning
        {
            get { return currentWorkSpaceHolder != null; }
        }

        public IBurrowConfig Configuration
        {
            get { return cfg; }
        }

        public void Start()
        {
            if (IsRunning)
            {
                throw new GeneralException("Burrow Environment is already running");
            }

            currentWorkSpaceHolder = new LocalSafe<WorkSpace>();
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
        /// Initialize the WorkSpace 
        /// </summary>
        /// <remarks>
        /// Please read the remarks of the <see cref="NHibernate.Burrow.Impl.WorkSpace"/>
        /// You normally don't need to call this method
        /// </remarks>
        /// <param name="states">
        /// Initialized the domain context with a collection of states
        /// </param>
        /// <param name="currentWorkSpaceName"></param>
        public void InitWorkSpace(NameValueCollection states, string currentWorkSpaceName)
        {
            CheckCurrentWorkSpace();

            CurrentWorkSpace = WorkSpace.Create(states, currentWorkSpaceName);
        }
 

        private void CheckCurrentWorkSpace()
        {
            if (CurrentWorkSpace != null)
            {
                throw new BurrowException("WorkSpace is already initialized");
            }
        }

        private void CheckRunning()
        {
            if (!IsRunning)
            {
                throw new FrameworkAlreadyShutDownException();
            }
        }
    }
}