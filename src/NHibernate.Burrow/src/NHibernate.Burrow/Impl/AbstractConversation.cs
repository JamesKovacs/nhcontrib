using System;
using System.Collections.Generic;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl
{
    ///<summary>
    /// Represents a conversation between user and the system
    ///</summary>
    /// <remarks>
    /// Actually you shouldn't need to use this class. We may hide it to internal in the future. 
    /// Currently we leave it public mainly for testing purpose. 
    /// </remarks>
    internal abstract class AbstractConversation : IConversation
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly IDictionary<string, object> items = new Dictionary<string, object>();
        private readonly GuidDataContainer safeItems = new GuidDataContainer();
        protected readonly IDictionary<PersistenceUnit, SessionManager> sessManagers =
            new Dictionary<PersistenceUnit, SessionManager>();
        private bool givenUp = false;
        private DateTime lastVisit = DateTime.Now;
        private SpanStrategy spanStrategy = SpanStrategy.DoNotSpan;
        private string workSpaceName;

        #region Constructors

        protected AbstractConversation()
        {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
            {
                sessManagers.Add(pu, CreateSessionManager(pu));
            }
            foreach (SessionManager sm in sessManagers.Values)
            {
                sm.OnConversationStarts();
            }
        }

        protected abstract SessionManager CreateSessionManager(PersistenceUnit pu);

        #endregion

        #region public methods

        /// <summary>
        /// Indicates if this convesation is in the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// Conversation in the <see cref="ConversationPool"/> will be persistent until they are closed.
        /// Conversation not in the <see cref="ConversationPool"/> will be garbage collected once the httpContext or thread is discarded. 
        /// </remarks>
        public bool IsInPool
        {
            get { return ConversationPool.Instance.ContainsKey(id); }
        }

        public SpanStrategy SpanStrategy
        {
            get
            {
                if (!IsInPool)
                {
                    return SpanStrategy.DoNotSpan;
                }
                return spanStrategy;
            }
            private set { spanStrategy = value; }
        }

        public bool GivenUp
        {
            get { return givenUp; }
        }

        /// <summary>
        /// Gets the <see cref="ITransactionManager"/> when in an Sticky WorkSpace.
        /// </summary>
        /// <remarks>
        /// For this property to be available, you must call 
        /// <see cref="BurrowFramework.InitStickyWorkSpace()"/> instead of
        /// <see cref="BurrowFramework.InitWorkSpace()"/> at the very begninning
        /// </remarks>
        public abstract ITransactionManager TransactionManager
        { get;
        }

        /// <summary>
        /// Gets the data bag the conversation holds
        /// </summary>
        /// <remarks>
        /// You can use this item to store conversation span data
        /// </remarks>
        public GuidDataContainer SafeItems
        {
            get
            {
                Visited();
                return safeItems;
            }
        }

        /// <summary>
        /// Gets the unique id of this conversation
        /// </summary>
        public Guid Id
        {
            get { return id; }
        }

        /// <summary>
        /// Start a long Coversation that spans over multiple http requests
        /// </summary>
        public bool SpanWithPostBacks()
        {
            return Span(SpanStrategy.Post, string.Empty);
        }

        /// <summary>
        /// Start a long Coversation that spans over the whole session
        /// </summary>
        /// <param name="inWorkSpaceName">span in the work space</param>
        public bool SpanWithCookie(string inWorkSpaceName)
        {
            return Span(SpanStrategy.Cookie, inWorkSpaceName);
        }

        /// <summary>
        /// Give up the data change made in this conversation and stop spanning
        /// </summary>
        /// <remarks>
        /// This won't imediately close the conversation, it tells the conversation not to commit the DB change when it is closed. 
        /// </remarks>
        public void GiveUp()
        {
            givenUp = true;
            StopSpanning();
        }

        /// <summary>
        /// tells the conversation to stop spanning itself and commit the data change made in it when it's closed
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// call this method when all operations in a long span conversation are successfully done
        /// </remarks>
        public bool FinishSpan()
        {
            if (givenUp)
            {
                return false;
            }
            StopSpanning();
            return true;
        }

        /// <summary>
        /// Gets if this conversation is Spanning (either with Postbacks or HttpSessions)
        /// </summary>
        public bool IsSpanning
        {
            get { return SpanStrategy.ValidForSpan; }
        }

        /// <summary>
        /// Gets the last time this conversation is visited
        /// </summary>
        public DateTime LastVisit
        {
            get { return lastVisit; }
            private set { lastVisit = value; }
        }

        public string WorkSpaceName
        {
            get { return workSpaceName; }
        }

        public IDictionary<string, object> Items
        {
            get { return items; }
        }

        #endregion

        #region IConversation Members

        public event EventHandler Closed;

        #endregion

        private void Visited()
        {
            LastVisit = DateTime.Now;
        }

        /// <summary>
        /// Add conversation to the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if already in the <see cref="ConversationPool"/>, do simply change the SpanStrategy if there is any change
        /// </remarks>
        private bool Span(SpanStrategy om, string inWorkSpaceName)
        {
            Visited();
            if (!om.ValidForSpan)
            {
                throw new ArgumentException(om + "is not an accetable overspan strategy for spanning");
            }
            if (GivenUp)
            {
                throw new ConversationAlreadyCancelledException();
            }
            SpanStrategy = om;
            bool retVal = false;
            if (!IsInPool)
            {
                ConversationPool.Instance.Add(id, this);
                retVal = true;
            }
            workSpaceName = inWorkSpaceName;
            WorkSpaceUpdate();
            return retVal;
        }

        /// <summary>
        /// Remove this from the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if not in the <see cref="ConversationPool"/>, do nothing
        /// </remarks>
        private void StopSpanning()
        {
            if (IsInPool)
            {
                ConversationPool.Instance.Remove(id);
            }
            SpanStrategy = SpanStrategy.DoNotSpan;
            WorkSpaceUpdate();
        }

        private void WorkSpaceUpdate()
        {
            if (WorkSpace.Current != null && Equals(WorkSpace.Current.Conversation))
            {
                WorkSpace.Current.UpdateToHttpContext();
            }
        }

        /// <summary>
        /// Commit the data changes happened in this conversation and close it.
        /// </summary>
        /// <remarks>
        /// The NHibernate session will also be discard after you close the conversation
        /// </remarks>
        private void CommitAndClose()
        {
            CheckState();

            try
            {
                foreach (SessionManager sm in sessManagers.Values)
                {
                    sm.OnConversationFinish();
                }
            }
            finally
            {
                CloseNHibernateSessions();
                Close();
            }
        }

        private void CheckState()
        {
            if (GivenUp)
            {
                throw new ConversationUnavailableException("Conversation was already canceld");
            }
        }
 


        /// <summary>
        /// immediately rollback 
        /// </summary>
        internal void RollbackAndClose()
        {
            try
            {
                foreach (SessionManager sm in sessManagers.Values)
                {
                    sm.OnConversationRollback();
                }
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Shortcut to flush the session
        /// </summary>
        public void Flush()
        {
            foreach (SessionManager sm in sessManagers.Values)
            {
                sm.GetSession().Flush();
            }
        }
 

         

        internal void CloseNHibernateSessions()
        {
            foreach (SessionManager sm in sessManagers.Values)
            {
                sm.CloseSession();
            }
        }

        /// <summary>
        /// The singleton Instance 
        /// </summary>
        internal SessionManager GetSessionManager()
        {
            return sessManagers[PersistenceUnitRepo.Instance.GetOnlyPU()];
        }

        internal SessionManager GetSessionManager(System.Type t)
        {
            return sessManagers[PersistenceUnitRepo.Instance.GetPU(t)];
        }

        internal void OnWorkSpaceClose()
        {
            if (GivenUp)
            {
                RollbackAndClose();
            }
            else if (!IsInPool)
            {
                CommitAndClose();
            }
        }

        private void Close()
        {
            try
            {
                StopSpanning();
            }
            finally
            {
                if (Closed != null)
                {
                    Closed(this, new EventArgs());
                }
            }
        }
 
    }
}