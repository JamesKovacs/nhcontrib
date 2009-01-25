using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl
{
    internal class FrameworkEnvironment : IFrameworkEnvironment
    {
        private static readonly FrameworkEnvironment instance = new FrameworkEnvironment();
        private IBurrowConfig cfg;
        private LocalSafe<WorkSpace> currentWorkSpaceHolder;

        private FrameworkEnvironment()
        {
            ReConfig(null);
        }

        public void ReConfig(IConfigurator configurator)
        {
            if(IsRunning)
                ShutDown();
            cfg = new ConfigurationFactory().Create(configurator);
            Start();
            cfg.Configurator = null;
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
        	PersistenceUnitRepo.ResetInstance();
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
			PersistenceUnitRepo.Initialize(Configuration);
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
 

		/// <summary>
		/// Get the NHibernate Configuration of a Persistence Unit
		/// </summary>
		/// <param name="persistenceUnitName">the name of the <see cref="PersistenceUnit"/></param>
		/// <returns></returns>
		/// <remarks>
		/// Please understand that the if you need to rebuild the sessionfactories after you changed the
		/// retrieved NHibernate Configure, please call <see cref="RebuildSessionFactories"/> 
		/// If you restart the Burrow Environment by calling <see cref="ShutDown"/> 
		/// and <see cref="Start"/>, your change will get lost.  
		/// </remarks>
		public NHibernate.Cfg.Configuration GetNHConfig(string persistenceUnitName) {
			return PersistenceUnitRepo.Instance.GetPU(persistenceUnitName).NHConfiguration;
		}

		/// <summary>
		/// Force all SessionFactory get rebuild
		/// </summary>
		public void RebuildSessionFactories() {
			foreach (PersistenceUnit unit in PersistenceUnitRepo.Instance.PersistenceUnits) {
				unit.ReBuildSessionfactory();
			}
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