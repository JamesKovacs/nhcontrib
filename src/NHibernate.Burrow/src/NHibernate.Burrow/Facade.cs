using System.Collections.Specialized;
using NHibernate.Burrow;

namespace NHibernate.Burrow {
    public static class Facade {
        /// <summary>
        /// prepare the NHibernate environment for the Domain Layer
        /// </summary>
        /// <remarks>
        /// This should be called before any NHibernate related operation
        /// </remarks>
        public static void InitializeDomain() {
            InitializeDomain(false, null);
        }

        public static void InitializeDomain(bool safe, NameValueCollection states) {
            if (safe && DomainContext.Current != null)
                CloseDomain();
            DomainContext.Initialize(states);
        }

        public static void InitializeDomain(NameValueCollection states) {
            InitializeDomain(false, states);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CloseDomain() {
            if (DomainContext.Current != null)
                DomainContext.Current.Close();
        }

        /// <summary>
        /// Start a long Coversation that spans over multiple http requests
        /// </summary>
        public static void StarLongConversation() {
            DomainContext.Current.StarLongConversation();
        }

        /// <summary>
        /// Start a long Coversation that spans over the whole session
        /// </summary>
        public static void StarSessionConversation() {
            DomainContext.Current.StarSessionConversation();
        }

        /// <summary>
        /// Finish a conversation that spans over multiple http requests and commit the data changes.
        /// </summary>
        public static void FinishOverSpanConversation() {
            DomainContext.Current.FinishConversation();
        }

        /// <summary>
        /// Cancel the current Conversation, so it won't be committed
        /// </summary>
        /// <remarks>
        /// calling this wont' immediately rollback the transaction
        /// </remarks>
        public static void CancelConversation() {
            DomainContext.Current.CancelConversation();
        }
    }
}