using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Impl {
	/// <summary>
	/// Represents the current work space
	/// </summary>
	/// <remarks>
	/// </remarks>
	internal sealed class WorkSpace {
		private const string ConversationIdKeyName = "_NHibernate.Burrow.ConversationId_";
		private bool closed = false;
		private ConversationImpl conversation;

		private WorkSpace(ConversationImpl c) {
			c.Closed += new EventHandler(conversation_Closed);
			conversation = c;
		}

		public SpanStrategy SpanStrategy {
			get {
				CheckConversation();
				return conversation.SpanStrategy;
			}
		}

		public Guid ConversationId {
			get {
				CheckConversation();
				return conversation.Id;
			}
		}

		public string WorkSpaceName {
			get {
				CheckConversation();
				return conversation.WorkSpaceName;
			}
		}

		public bool Closed {
			get { return closed; }
		}

		/// <summary>
		/// The current WorkSpace your code is working in
		/// </summary>
		public static WorkSpace Current {
			get { return FrameworkEnvironment.Instance.CurrentWorkSpace; }
		}

		public ConversationImpl Conversation {
			get { return conversation; }
		}

		public static WorkSpace Initialize(NameValueCollection states, string currentWorkSpaceName) {
			string cid = GetWorkSpaceState(states, ConversationIdKeyName, currentWorkSpaceName);
			Guid gcid = !string.IsNullOrEmpty(cid) ? new Guid(cid) : Guid.Empty;
			return new WorkSpace(InitializeConversation(gcid));
		}

		private static ConversationImpl InitializeConversation(Guid cid) {
			if (cid != Guid.Empty)
				return ConversationPool.Instance[cid];
			return ConversationImpl.StartNew();
		}

		private void CheckConversation() {
			if (Conversation == null)
				throw new ConversationUnavailableException("Conversation is probably already closed");
		}

		private static string GetWorkSpaceState(NameValueCollection states, string key, string currentWorkSpaceName) {
			string retVal = GetState(states, key + currentWorkSpaceName);
			if (string.IsNullOrEmpty(retVal))
				retVal = GetState(states, key);
			return retVal;
		}

		private IDictionary<string, string> SpanStates()
		{
			IDictionary<string, string> retVal = new Dictionary<string, string>();
			retVal.Add(ConversationIdKeyName + WorkSpaceName, conversation.Id.ToString());
			return retVal;
		}

		private static string GetState(NameValueCollection states, string key) {
			if (states != null) {
				string state = states[key];
				if (state != null) {
					string[] cids = state.Split(',');
					foreach (string cid in cids)
						if (!string.IsNullOrEmpty(cid))
							return cid;
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="originalUrl"></param>
		/// <returns></returns>
		public string WrapUrlWithSpanInfo(string originalUrl) {
			StringBuilder sb = new StringBuilder(originalUrl);
			foreach (KeyValuePair<string, string> p in SpanStates())
				Append(sb, p.Key, p.Value);
			return sb.ToString();
		}

	

		private void Append(StringBuilder sb, string key, string val) {
			bool firstPara = sb.ToString().IndexOf("?") < 0;
			if (SpanStrategy.ValidForSpan && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
				sb.Append((firstPara ? "?" : "&"));
				sb.Append(UrlEncode(key));
				sb.Append("=");
				sb.Append(UrlEncode(val));
			}
		}

		private void conversation_Closed(object sender, EventArgs e) {
			conversation = null;
		}

		/// <summary>
		/// Close the current domain context. call this after you are done with all the domain operations that requires interaction with NHibernate
		/// </summary>
		/// <remarks>
		/// Please read the remarks of the <see cref="NHibernate.Burrow.Impl.WorkSpace"/>
		/// </remarks>
		public void Close() {
			if (closed)
				return;
			if (conversation != null)
				conversation.OnWorkSpaceClose();
			closed = true;
		}

		private static string UrlEncode(string s) {
			return HttpUtility.UrlEncode(s);
		}

		public void UpdateToHttpContext() {
			HttpContext c = HttpContext.Current;
			if (c == null) return;
			foreach (KeyValuePair<string, string> p in SpanStates())
				SpanStrategy.UpdateSpanStates(c, p.Key, p.Value);
		}

		public void UpdateToControl(Control c) {
			foreach (KeyValuePair<string, string> pair in SpanStates())
				SpanStrategy.AddOverspanStateWhenRendering(c, pair.Key, pair.Value);
		}

		#region private memebers

		#endregion

	    public static NameValueCollection CreateState(Guid id, string workSpaceName) {
	        NameValueCollection nvc = new NameValueCollection();
            nvc.Add(ConversationIdKeyName+workSpaceName, id.ToString());
	        return nvc;
        }
	}
}