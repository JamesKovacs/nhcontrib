using System;
using System.Web;
using System.Web.UI;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.NHDomain.Exceptions;

namespace NHibernate.Burrow.WebUtil {
    /// <summary>
    /// </summary>
    public class WebUtilHTTPModule : IHttpModule {
        private static DomainContext CurrentDomainContext {
            get {
                if (DomainContext.Current == null)
                    throw new DomainContextUninitializedException();
                return DomainContext.Current;
            }
        }

        #region IHttpModule Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context) {
            context.PreRequestHandlerExecute += new EventHandler(BeginContext);
            context.PostRequestHandlerExecute += new EventHandler(CloseContext);
            context.Error += new EventHandler(RollBack);
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
        private void RollBack(object sender, EventArgs e) {
            CurrentDomainContext.CancelConversation();
            CurrentDomainContext.Close();
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private void BeginContext(object sender, EventArgs e) {
            HttpApplication ctx = (HttpApplication) sender;
            if (HandlerIsIrrelavant(ctx))
                return;
          
            if (DomainContext.Current != null)
                DomainContext.Current.Close();
            //Close the DomainContext remained in the memory the last request failed to close
            DomainContext.Initialize(ctx.Request.Params);
            if (ctx.Context.Handler is Page) {
                Page p = (Page) ctx.Context.Handler;
                new StatefulFieldPageModule(p);
                new ConversationStatePageModule(p);
            }
        }

        private bool HandlerIsIrrelavant(HttpApplication ctx) {
            return ctx.Context.Handler is System.Web.Handlers.AssemblyResourceLoader
                   || ctx.Context.Handler is System.Web.DefaultHttpHandler;
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="SessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CloseContext(object sender, EventArgs e) {
            HttpApplication ctx = (HttpApplication)sender;
            if (HandlerIsIrrelavant(ctx))
                return;
            CurrentDomainContext.Close();
        }

        ~WebUtilHTTPModule() {
            if (DomainContext.Current != null)
                DomainContext.Current.Close();
        }
    }
}