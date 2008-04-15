using System;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil
{
    /// <summary>
    /// </summary>
    public class WebUtilHTTPModule : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(BeginContext);
            context.PostRequestHandlerExecute += new EventHandler(CloseContext);
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
            if (!new Facade().Alive)
            {
                return;
            }
            try
            {
                new Facade().CurrentConversation.GiveUp();
                new Facade().CloseDomain();
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
            new Facade().InitializeDomain(true, ctx.Request.Params, currentWorkSpaceName);
            if (handler is Page)
            {
                Page p = (Page) handler;
                p.Init += new EventHandler(p_Init);
                new StatefulFieldPageModule(p);
                new ConversationStatePageModule(p);
            }
        }

    	private IWorkSpaceNameSniffer Sniffer() {
    		IBurrowConfig cfg = new Facade().BurrowEnvironment.Configuration;
			if (string.IsNullOrEmpty(cfg.WorkSpaceNameSniffer))
				return new Impl.WorkSpaceSnifferByAttribute();
			else
				return Burrow.Util.InstanceLoader.Load<IWorkSpaceNameSniffer>(cfg.WorkSpaceNameSniffer);

    	}

    	private void p_Init(object sender, EventArgs e)
        {
            ScriptManager current = ScriptManager.GetCurrent((Page) sender);
            if (current != null && current.EnablePartialRendering)
            {
                current.AsyncPostBackError +=  AsyncPostBackError ;
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
            HttpApplication ctx = (HttpApplication) sender;
            if (HandlerIsIrrelavant(ctx))
            {
                return;
            }
            new Facade().CloseDomain();
        }
    }
}