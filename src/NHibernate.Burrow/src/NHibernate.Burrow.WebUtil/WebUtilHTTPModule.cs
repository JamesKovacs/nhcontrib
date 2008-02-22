using System;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;
using NHibernate.Burrow;

namespace NHibernate.Burrow.WebUtil {
    /// <summary>
    /// </summary>
    public class WebUtilHTTPModule : IHttpModule {
        #region IHttpModule Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context) {
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
        private static void OnError(object sender, EventArgs e) {
            if(!Facade.Alive) return;
            try
            {
                Facade.CancelConversation();
                Facade.CloseDomain();
            }
            catch (Exception)
            {
                //Catch the exception during roll back 
            }
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private void BeginContext(object sender, EventArgs e) {
            HttpApplication ctx = (HttpApplication) sender;
            if (HandlerIsIrrelavant(ctx))
                return;

            Facade.InitializeDomain(true, ctx.Request.Params);
            if (ctx.Context.Handler is Page) {
                Page p = (Page) ctx.Context.Handler;
                new StatefulFieldPageModule(p);
                new ConversationStatePageModule(p);
            }
        }

        private bool HandlerIsIrrelavant(HttpApplication ctx) {
            return ctx.Context.Handler is AssemblyResourceLoader
                   || ctx.Context.Handler is DefaultHttpHandler;
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="SessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CloseContext(object sender, EventArgs e) {
            HttpApplication ctx = (HttpApplication) sender;
            if (HandlerIsIrrelavant(ctx))
                return;
            Facade.CloseDomain();
        }

        ~WebUtilHTTPModule() {
            Facade.CloseDomain();
        }
    }
}