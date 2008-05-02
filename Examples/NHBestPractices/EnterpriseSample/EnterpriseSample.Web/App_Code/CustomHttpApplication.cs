using System;
using System.Web;
using log4net.Config;

namespace EnterpriseSample.Web
{
    /// <summary>
    /// Summary description for IpcsHttpApplication
    /// </summary>
    public class CustomHttpApplication : HttpApplication
    {
        /// <summary>
        /// Code that runs on application startup
        /// </summary>
        public void Application_Start(object sender, EventArgs e)
        {
            // Initialize log4net
            XmlConfigurator.Configure();
        }

        public void Application_End(object sender, EventArgs e) { }

        public void Application_Error(object sender, EventArgs e) { }

        public void Session_Start(object sender, EventArgs e) { }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The Session_End event is raised only when the sessionstate mode
        /// is set to InProc in the Web.config file. If session mode is set to StateServer 
        /// or SQLServer, the event is not raised.
        /// </remarks>
        public void Session_End(object sender, EventArgs e) { }
    }
}