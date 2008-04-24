using System;
using System.Web;
using System.Web.Handlers;
using System.Web.UI;
using NHibernate.Burrow.Util;
 
 
    /// <summary>
    /// </summary>
    public class CustomHTTPModule : IHttpModule
    {
	
		public static bool Enabled = false;
        #region IHttpModule Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
       
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
			if (Enabled) {
				HttpApplication ctx = (HttpApplication)sender;
				Exception objErr = ctx.Server.GetLastError().GetBaseException();
				if(objErr is CustomException)
					((HttpApplication)sender).Response.Redirect("~/CustomErrorHandling/result.aspx");
			}
        }
 
    }
