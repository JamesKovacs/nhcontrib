using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using NHibernate.Burrow.DataContainers;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow.Impl {
    /// <summary>
    /// Represents the MindLib managed NHibernate environment within which your domain layer worksDomain Context
    /// You can aslo deem this as the counterpart of HttpContext within which your asp.net layer works. 
    /// </summary>
    /// <remarks>
    /// The DomainContext contains the status of a particular managed environment for a particular request or thread. 
    /// It has to be <see cref="Initialize()"/> before MindLib can help management your NHibernate environtment. 
    /// Also it needs to be <see cref="Close()"/> after that httpRequest or thread is done. 
    /// This initialization and close is normally automatically taken care of by MindLib itself for a Asp.net application. 
    /// But for Unit Test or you need use your domain layer in a separate thread, you will need to call the <see cref="Initialize()"/>
    /// and <see cref="Close"/> before and after your domain layer operations. 
    /// There is also instruction on how to deal with these scenarios in the documentation
    /// </remarks>
    internal sealed class DomainContext {
        private static LocalSafe<DomainContext> current = new LocalSafe<DomainContext>();

        private DomainContext() {}

        #region private memebers

        private ConversationImpl CurrentConversation {
            get {
                Facade f = new Facade();
                
                if (f.CurrentConversation == null)
                    throw new ConversationUninitializedException();
                return (ConversationImpl)f.CurrentConversation;
            }
        }

        #endregion

        /// <summary>
        /// The current DomainContext your code is working in
        /// </summary>
        public static DomainContext Current {
            get { return current.Value; }
        }

        /// <summary>
        /// Initialize a Domain Context 
        /// </summary>
        /// <remarks>
        /// Please read the remarks of the <see cref="DomainContext"/>
        /// This should be called before any NHibernate related operation
        /// </remarks>
        public static void Initialize() {
            Initialize(new NameValueCollection());
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
        public static void Initialize(NameValueCollection states) {
            if (current.Value == null)
                current.Value = new DomainContext();
            else
                throw new BurrowException("DomainContext is already initialized");
            InitializeConversation(states);
        }

        private static void InitializeConversation(NameValueCollection states) {
            if (states != null) {
                string cid = states[ConversationImpl.ConversationIdKeyName];
                if (!string.IsNullOrEmpty(cid)) {
                    FrameworkEnvironment.Instance.RetrieveCurrent(new Guid(cid));
                    return;
                }
            }
            FrameworkEnvironment.Instance.StartNewConversation();
        }

        /// <summary>
        /// Start a long Coversation that spans over multiple http requests
        /// </summary>
        public void StarLongConversation() {
            CurrentConversation.AddToPool(SpanStrategy.Post);
        }

        /// <summary>
        /// Start a long Coversation that spans over the whole session
        /// </summary>
        public void StarSessionConversation() {
            CurrentConversation.AddToPool(SpanStrategy.Cookie);
        }

      

        /// <summary>
        /// Cancel the current Conversation, so it won't be commit
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void CancelConversation() {
            CurrentConversation.GiveUp();
        }

        /// <summary>
        /// Close the current domain context. call this after you are done with all the domain operations that requires interaction with NHibernate
        /// </summary>
        /// <remarks>
        /// Please read the remarks of the <see cref="DomainContext"/>
        /// </remarks>
        public void Close() {
            try {
                if (new Facade().CurrentConversation  != null)
                    CurrentConversation.OnDomainContextClose();
            }
            finally {
                current.Value = null;
            }
        }

        /// <summary>
        /// States that should over span multiple domain context 
        /// </summary>
        /// <returns></returns>
        public IList<SpanState> SpanStates() {
            IList<SpanState> retVal = new List<SpanState>();
            retVal.Add(
                new SpanState(ConversationImpl.ConversationIdKeyName, CurrentConversation.Id.ToString(),
                                  CurrentConversation.SpanStrategy));
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        public  string WrapUrlWithSpanInfo(string originalUrl)
        {
            StringBuilder sb = new StringBuilder(originalUrl);

            bool firstPara = originalUrl.IndexOf("?") < 0;
            foreach (SpanState state in  SpanStates())
            {
                if (state.Strategy.ValidForSpan && !string.IsNullOrEmpty(state.Name)
                    && !string.IsNullOrEmpty(state.Value))
                {
                    sb.Append((firstPara ? "?" : "&"));
                    sb.Append(UrlEncode(state.Name));
                    sb.Append("=");
                    sb.Append(UrlEncode(state.Value));
                    firstPara = false;
                }
            }
            return sb.ToString();
        }

        private string UrlEncode(string s) {
            return System.Web.HttpUtility.UrlEncode(s);
        
        }
    }
}