using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace NHibernate.Burrow.Impl {
    /// <summary>
    /// Represents the MindLib managed NHibernate environment within which your domain layer worksDomain Context
    /// You can aslo deem this as the counterpart of HttpContext within which your asp.net layer works. 
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal sealed class DomainContext {
        private bool closed = false;
        private ConversationImpl currentConversation;

        #region private memebers

        #endregion

        private DomainContext(NameValueCollection states) {
            InitializeConversation(states);
        }

        public bool Closed {
            get { return closed; }
        }

        /// <summary>
        /// The current DomainContext your code is working in
        /// </summary>
        public static DomainContext Current {
            get { return FrameworkEnvironment.Instance.CurrentDomainContext; }
        }

        public ConversationImpl CurrentConversation {
            get { return currentConversation; }
            private set { currentConversation = value; }
        }

        public static DomainContext StartNew(NameValueCollection states) {
            DomainContext retVal = new DomainContext(states);
            return retVal;
        }

        private void InitializeConversation(NameValueCollection states) {
            if (states != null) {
                string state = states[ConversationImpl.ConversationIdKeyName];
                if (state != null) {
                    string[] cids = state.Split(',');
                    foreach (string cid in cids) {
                        if (!string.IsNullOrEmpty(cid)) {
                            SetCurrentConversation(ConversationPool.Instance[new Guid(cid)]);
                            return;
                        }
                    }
                }
            }
            SetCurrentConversation(ConversationImpl.StartNew());
        }

        private void SetCurrentConversation(ConversationImpl c) {
            c.Closed += new EventHandler(conversation_Closed);
            currentConversation = c;
            return;
        }

        private void conversation_Closed(object sender, EventArgs e) {
            currentConversation = null;
        }


        /// <summary>
        /// Cancel the current Conversation, so it won't be commit
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void CancelConversation() {
            currentConversation.GiveUp();
        }

        /// <summary>
        /// Close the current domain context. call this after you are done with all the domain operations that requires interaction with NHibernate
        /// </summary>
        /// <remarks>
        /// Please read the remarks of the <see cref="DomainContext"/>
        /// </remarks>
        public void Close() {
            if (closed)
                return;
            if (currentConversation != null)
                currentConversation.OnDomainContextClose();
            closed = true;
        }

        /// <summary>
        /// States that should over span multiple domain context 
        /// </summary>
        /// <returns></returns>
        public IList<SpanState> SpanStates() {
            IList<SpanState> retVal = new List<SpanState>();
            retVal.Add(
                new SpanState(ConversationImpl.ConversationIdKeyName, currentConversation.Id.ToString(),
                              currentConversation.SpanStrategy));
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        public string WrapUrlWithSpanInfo(string originalUrl) {
            StringBuilder sb = new StringBuilder(originalUrl);

            bool firstPara = originalUrl.IndexOf("?") < 0;
            foreach (SpanState state in  SpanStates())
                if (state.Strategy.ValidForSpan && !string.IsNullOrEmpty(state.Name)
                    && !string.IsNullOrEmpty(state.Value)) {
                    sb.Append((firstPara ? "?" : "&"));
                    sb.Append(UrlEncode(state.Name));
                    sb.Append("=");
                    sb.Append(UrlEncode(state.Value));
                    firstPara = false;
                }
            return sb.ToString();
        }

        private string UrlEncode(string s) {
            return HttpUtility.UrlEncode(s);
        }
    }
}