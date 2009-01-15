using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;
using NHibernate.Burrow.Util;
using NHibernate.Burrow.WebUtil.Impl;

namespace NHibernate.Burrow.WebUtil
{
    /// <summary>
    /// </summary>
    public class WebUtilHTTPModule : IHttpModule
    {
        private static BurrowFramework bf = new BurrowFramework();

        #region IHttpModule Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(BeginContext);
            context.EndRequest += new EventHandler(CloseContext);
            context.Error += new EventHandler(OnError);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {}

        #endregion

        /// <summary>
        /// Rollback the transactions when error occurs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnError(object sender, EventArgs e)
        {
            if (!bf.WorkSpaceIsReady)
            {
                return;
            }
            try
            {
                bf.CurrentConversation.GiveUp();
                bf.CloseWorkSpace();
            }
            catch (Exception)
            {
                //Catch the exception during roll back so that Error Exception won't get swallowed.
            }
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private void BeginContext(object sender, EventArgs e)
        {
            HttpApplication ctx = (HttpApplication) sender;
            if (HandlerIsIrrelavant(ctx))
            {
                return;
            }
            IHttpHandler handler = ctx.Context.Handler;
            string currentWorkSpaceName = Sniffer().Sniff(handler);

            bf.InitWorkSpace(true, GetParams(ctx.Request), currentWorkSpaceName);
            if (handler is Page)
            {
                Page p = (Page) handler;
                p.Init += new EventHandler(p_Init);
                GlobalPlaceHolder gph = new GlobalPlaceHolder(p);
               new StatefulFieldPageModule(p, gph);
               new ConversationStatePageModule(p, gph);
            }
        }

		/// <summary>
		/// Excludes the query string when in PostBack
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
    	private NameValueCollection GetParams(HttpRequest request) {
			NameValueCollection nvc = new NameValueCollection(request.Form);
    		nvc.Add(request.ServerVariables);
			foreach (string key in request.Cookies.AllKeys) {
    			nvc.Add(key,request.Cookies[key].Value);
    		}
			if(request.HttpMethod.ToUpper().Trim() != "POST" )
				nvc.Add(request.QueryString);
    		return nvc;

		}

    	private IWorkSpaceNameSniffer Sniffer()
        {
            IBurrowConfig cfg = bf.BurrowEnvironment.Configuration;
            if (string.IsNullOrEmpty(cfg.WorkSpaceNameSniffer))
            {
                return new WorkSpaceSnifferByAttribute();
            }
            else
            {
                return InstanceLoader.Load<IWorkSpaceNameSniffer>(cfg.WorkSpaceNameSniffer);
            }
        }

        private void p_Init(object sender, EventArgs e)
        {
            ScriptManager current = ScriptManager.GetCurrent((Page) sender);
            if (current != null && current.EnablePartialRendering)
            {
                current.AsyncPostBackError += AsyncPostBackError;
            }
        }

        private static void AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            OnError(sender, e);
        }

        private bool HandlerIsIrrelavant(HttpApplication ctx)
        {
            return ctx.Context.Handler is AssemblyResourceLoader || ctx.Context.Handler is DefaultHttpHandler;
        }

        /// <summary>
        /// </summary>
        private void CloseContext(object sender, EventArgs e)
        {
            bf.CloseWorkSpace();
        }
    }
}