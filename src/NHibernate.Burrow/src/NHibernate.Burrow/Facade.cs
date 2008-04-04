using System.Collections.Specialized;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow {
    public static class Facade {
        public static IConversation CurrentConversation {
            get { return ConversationImpl.Current; }
        }

        public static IBurrowConfig Configuration {
            get { return  NHibernateBurrowCfgSection.GetInstance(); }
        }

        /// <summary>
        /// gets if the domain layer is alive. 
        /// </summary>
        public static bool Alive {
            get { return DomainContext.Current != null; }
        }

        /// <summary>
        /// prepare the NHibernate environment for the Domain Layer
        /// </summary>
        /// <remarks>
        /// This should be called before any NHibernate related operation
        /// </remarks>
        public static void InitializeDomain() {
            InitializeDomain(false, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="safe">close the Domain in the current context first if there is any.</param>
        /// <param name="states"></param>
        public static void InitializeDomain(bool safe, NameValueCollection states) {
            if (safe && Alive)
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
        /// Gets the managed NHibernate ISession
        /// </summary>
        /// <returns></returns>
        public static ISession GetSession() {
            return SessionManager.GetInstance().GetSession();
        }

        /// <summary>
        /// Gets a managed ISession
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        /// <remarks>
        /// Please do not try to close or commit transaction of this session as its status and transaction are controlled by Burrow
        /// </remarks>
        public static ISession GetSession(System.Type entityType) {
            return SessionManager.GetInstance(entityType).GetSession();
        }

        public static string WrapUrlWithConversationInfo(string originalUrl) {
            if (CurrentConversation.IsSpanning)
                return DomainContext.Current.WrapUrlWithSpanInfo(originalUrl);
            else
                throw new Exceptions.IncorrectConversationSpanStatusException(
                    "CurrentConversation Must Be In Span Before you can wrap a url");
        }
    }
}