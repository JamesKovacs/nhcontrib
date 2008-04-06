using System;
using System.Collections.Generic;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
    ///<summary>
    /// Represents a conversation between user and the system
    ///</summary>
    /// <remarks>
    /// Actually you shouldn't need to use this class. We may hide it to internal in the future. 
    /// Currently we leave it public mainly for testing purpose. 
    /// </remarks>
    internal class ConversationImpl : IConversation {
        public event System.EventHandler Closed;
        public const string ConversationIdKeyName = "NHibernate.Burrow.ConversationId";
       
        private readonly Guid id = Guid.NewGuid();
        private readonly GuidDataContainer items = new GuidDataContainer();

        private readonly IDictionary<PersistenceUnit, SessionManager> sessManagers =
            new Dictionary<PersistenceUnit, SessionManager>();

        private bool canceled = false;
        private DateTime lastVisit = DateTime.Now;
        private SpanStrategy spanStrategy = SpanStrategy.Post;

        private ConversationImpl() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits)
                sessManagers.Add(pu, new SessionManager(pu));
        }

        #region public methods


        /// <summary>
        /// Gets the data bag the conversation holds
        /// </summary>
        /// <remarks>
        /// You can use this item to store conversation span data
        /// </remarks>
        public GuidDataContainer Items {
            get {
                Visited();
                return items;
            }
        }

        /// <summary>
        /// Indicates if this convesation is in the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// Conversation in the <see cref="ConversationPool"/> will be persistent until they are closed.
        /// Conversation not in the <see cref="ConversationPool"/> will be garbage collected once the httpContext or thread is discarded. 
        /// </remarks>
        public bool IsInPool {
            get { return ConversationPool.Instance.ContainsKey(id); }
        }

        public SpanStrategy SpanStrategy {
            get {
                if (!IsInPool)
                    return SpanStrategy.DoNotSpan;
                return spanStrategy;
            }
            private set { spanStrategy = value; }
        }

        public bool Canceled {
            get { return canceled; }
        }

        /// <summary>
        /// Gets the unique id of this conversation
        /// </summary>
        public Guid Id {
            get { return id; }
        }

        /// <summary>
        /// Start a long Coversation that spans over multiple http requests
        /// </summary>
        public bool SpanWithPostBacks() {
            return AddToPool(SpanStrategy.Post);
        }

        /// <summary>
        /// Start a long Coversation that spans over the whole session
        /// </summary>
        public bool SpanWithHttpSession() {
            return AddToPool(SpanStrategy.Cookie);
        }

        /// <summary>
        /// Give up the data change made in this conversation and stop spanning
        /// </summary>
        /// <remarks>
        /// This won't imediately close the conversation, it tells the conversation not to commit the DB change when it is closed. 
        /// </remarks>
        public void GiveUp() {
            canceled = true;
            RemoveFromPool();
        }

        /// <summary>
        /// tells the conversation to stop spanning itself and commit the data change made in it when it's closed
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// call this method when all operations in a long span conversation are successfully done
        /// </remarks>
        public bool FinishSpan() {
            if (canceled)
                return false;
            RemoveFromPool();
            return true;
        }

        /// <summary>
        /// Gets if this conversation is Spanning (either with Postbacks or HttpSessions)
        /// </summary>
        public bool IsSpanning {
            get { return SpanStrategy.ValidForSpan; }
        }

        private void Visited() {
            LastVisit = DateTime.Now;
        }

       

        /// <summary>
        /// Add conversation to the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if already in the <see cref="ConversationPool"/>, do nothing
        /// </remarks>
        public bool AddToPool(SpanStrategy om) {
            Visited();
            if (!IsInPool && !Canceled) {
                if (!om.ValidForSpan)
                    throw new ArgumentException(
                        om + "is not an accetable overspan strategy for long conversation");
                SpanStrategy = om;

                ConversationPool.Instance.Add(id, this);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove this from the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if not in the <see cref="ConversationPool"/>, do nothing
        /// </remarks>
        public void RemoveFromPool() {
            if (IsInPool) ConversationPool.Instance.Remove(id);
        }

        /// <summary>
        /// Commit the data changes happened in this conversation and close it.
        /// </summary>
        /// <remarks>
        /// The NHibernate session will also be discard after you close the conversation
        /// </remarks>
        public void CommitAndClose() {
            CheckState();

            try {
                CommitNHibernateTransactions();
            }
            finally {
                CloseNHibernateSessions();
                Close();
            }
        }

        private void CheckState() {
            if (Canceled) throw new ConversationUnavailableException("Conversation was already canceld");
        }

        /// <summary>
        /// Force commit the data 
        /// </summary>
        /// <remarks>
        /// Call it for real special cases as it will break the atomicity of the conversation
        /// basically it will commit a database transaction and start a new one. 
        /// Multiple DB transaction in one conversation actually does not follow the design intention. 
        /// </remarks>
        public void ForceCommitChange() {
            Visited();
            CheckState();
            try {
                CommitNHibernateTransactions();
            }
            catch (Exception) {
                Close();
                throw;
            }
            BeginNHibernateTransactions();
        }

        /// <summary>
        /// immediately rollback 
        /// </summary>
        public void RollbackAndClose() {
            try {
                RollbackNHibernateTransacitons();
            }
            finally {
                Close();
            }
        }

        /// <summary>
        /// Shortcut to flush the session
        /// </summary>
        public void Flush() {
            foreach (SessionManager sm in sessManagers.Values)
                sm.GetSession().Flush();
        }

        /// <summary>
        /// Begin Transactions for all SessionManagers in All PersistenceUnits
        /// </summary>
        internal void BeginNHibernateTransactions() {
            foreach (SessionManager sm in sessManagers.Values)
                sm.BeginTransaction();
        }

        /// <summary>
        /// Commit Transactions for all SessionManagers in All PersistenceUnits
        /// </summary>
        internal void CommitNHibernateTransactions() {
            foreach (SessionManager sm in sessManagers.Values)
                sm.CommitTransaction();
        }

        internal void RollbackNHibernateTransacitons() {
            foreach (SessionManager sm in sessManagers.Values)
                sm.RollbackTransaction();
        }

        internal void CloseNHibernateSessions() {
            foreach (SessionManager sm in sessManagers.Values)
                sm.CloseSession();
        }

        /// <summary>
        /// The singleton Instance 
        /// </summary>
        internal SessionManager GetSessionManager() {
            return sessManagers[PersistenceUnitRepo.Instance.GetOnlyPU()];
        }

        internal SessionManager GetSessionManager(System.Type t) {
            return sessManagers[PersistenceUnitRepo.Instance.GetPU(t)];
        }

        #endregion

        #region nonpublic methods

        internal void OnDomainContextClose() {
            if (Canceled)
                RollbackAndClose();
            else if (!IsInPool)
                CommitAndClose();
        }

        private void Close() {
            try {
                RemoveFromPool();
            }
            finally {
                if (Closed != null)
                    Closed(this, new System.EventArgs());
            }
        }

        #endregion

        public DateTime LastVisit {
            get { return lastVisit; }
            private set { lastVisit = value; }
        }

        public static IConversation StartNew() {
            ConversationImpl ci =  new ConversationImpl();
            ci.BeginNHibernateTransactions();
            return ci;
        }

        public static ConversationImpl Current {
            get { return new Facade().CurrentConversation as ConversationImpl; }
        }
    }
}