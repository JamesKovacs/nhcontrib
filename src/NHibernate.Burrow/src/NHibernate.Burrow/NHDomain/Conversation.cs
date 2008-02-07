using System;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.NHDomain.Exceptions;

namespace NHibernate.Burrow.NHDomain {
    ///<summary>
    /// Represents a conversation between user and the system
    ///</summary>
    /// <remarks>
    /// Actually you shouldn't need to use this class. We may hide it to internal in the future. 
    /// Currently we leave it public mainly for testing purpose. 
    /// </remarks>
    public class Conversation {
        private static LocalSafe<Conversation> current = new LocalSafe<Conversation>();

        private readonly GuidDataContainer items = new GuidDataContainer();
        private bool canceled = false;
        private Guid id = Guid.Empty;
        private OverspanMode overspanMode = OverspanMode.Post;

        private Conversation() {}

        #region public methods

        /// <summary>
        /// The current context conversation
        /// </summary>
        public static Conversation Current {
            get { return current.Value; }
        }

        /// <summary>
        /// Gets the data bag the conversation holds
        /// </summary>
        /// <remarks>
        /// You can use this item to store conversation span data
        /// </remarks>
        public GuidDataContainer Items {
            get { return items; }
        }

        /// <summary>
        /// Gets the unique id of this conversation
        /// </summary>
        public Guid Id {
            get { return id; }
        }

        /// <summary>
        /// Indicates if this convesation is in the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// Conversation in the <see cref="ConversationPool"/> will be persistent until they are closed.
        /// Conversation not in the <see cref="ConversationPool"/> will be garbage collected once the httpContext or thread is discarded. 
        /// </remarks>
        public bool IsInPool {
            get { return id != Guid.Empty; }
        }

        public OverspanMode OverspanMode {
            get {
                if (!IsInPool)
                    return OverspanMode.None;
                return overspanMode;
            }
            private set { overspanMode = value; }
        }

        public bool Canceled {
            get { return canceled; }
        }

        /// <summary>
        /// Retreive a conversation by <paramref name="id"/> and make it the <see cref="Current"/>
        /// </summary>
        /// <param name="id"></param>
        public static void RetrieveCurrent(Guid id) {
            current.Value = ConversationPool.Instance[id];
        }

        /// <summary>
        /// Start a new conversation
        /// </summary>
        public static void StartNew() {
            current.Value = new Conversation();
            SessionManager.BeginNHibernateTransactions();
        }

        /// <summary>
        /// Add conversation to the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if already in the <see cref="ConversationPool"/>, do nothing
        /// </remarks>
        public void AddToPool(OverspanMode om) {
            if (!IsInPool) {
                if (om == OverspanMode.None)
                    throw new ArgumentException(
                        "OverspanMode.None is not an accetable overspan mode for adding conversation to pool");
                OverspanMode = om;
                id = Guid.NewGuid();
                ConversationPool.Instance.Add(id, this);
            }
        }

        /// <summary>
        /// Remove this from the <see cref="ConversationPool"/>
        /// </summary>
        /// <remarks>
        /// if not in the <see cref="ConversationPool"/>, do nothing
        /// </remarks>
        public void RemoveFromPool() {
            if (IsInPool) {
                ConversationPool.Instance.Remove(id);
                id = Guid.Empty;
            }
        }

        /// <summary>
        /// Commit the data changes happened in this conversation and close it.
        /// </summary>
        /// <remarks>
        /// The NHibernate session will also be discard after you close the conversation
        /// </remarks>
        public void CommitAndClose() {
            CheckState();

            //SessionManager.Flush(); 
            //it's added here because sometime transaction committing does not automatically flush the session especially when deleting something.
            //1-29-08 by Kai: but it will be flushed in the following method 
            try {
                SessionManager.CommitNHibernateTransactions();
            }
            finally {
                SessionManager.CloseNHibernateSessions();
                Close();
            }
        }

        private void CheckState() {
            if (Canceled) throw new ConversationUnavailableException("Conversation was already canceld");
        }

        /// <summary>
        /// commit the data change prior to this point and start a new transaction from now on.
        /// </summary>
        public void CommitCurrentChange() {
            CheckState();
            try {
                SessionManager.CommitNHibernateTransactions();
            }
            catch (Exception) {
                Close();
                throw;
            }
            SessionManager.BeginNHibernateTransactions();
        }

        public void RollbackAndClose() {
            try {
                SessionManager.RollbackNHibernateTransacitons();
            }
            finally {
                Close();
            }
        }

        /// <summary>
        /// Cancel all the data change happened in this conversation.
        /// </summary>
        public void Cancel() {
            canceled = true;
            RemoveFromPool();
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
                current.Value = null;
            }
        }

        #endregion
    }
}