using System;
using System.Web;
using log4net.Config;
//using Castle.Windsor;

namespace EnterpriseSample.Web
{
    /// <summary>
    /// Summary description for IpcsHttpApplication
    /// </summary>
    public class CustomHttpApplication : HttpApplication //, IContainerAccessor
    {
        /// <summary>
        /// Implements <see cref="IContainerAccessor" /> so that Castle facilities
        /// can gain access to the <see cref="HttpApplication" />.
        /// </summary>
        //public IWindsorContainer Container {
        //    get { return windsorContainer; }
        //}

        /// <summary>
        /// Provides a globally available access to the <see cref="IWindsorContainer" /> instance.
        /// </summary>
        //public static IWindsorContainer WindsorContainer {
        //    get { return windsorContainer; }
        //}

        /// <summary>
        /// Code that runs on application startup
        /// </summary>
        public void Application_Start(object sender, EventArgs e) {
            // Initialize log4net
            XmlConfigurator.Configure();

            // Create the Windsor Container for IoC.
            // Supplying "XmlInterpreter" as the parameter tells Windsor 
            // to look at web.config for any necessary configuration.
            //windsorContainer = new WindsorContainer(new XmlInterpreter());
        }

        public void Application_End(object sender, EventArgs e) {
            //windsorContainer.Dispose();
        }

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
        public void Session_End(object sender, EventArgs e) {
        }

        /// <summary>
        /// Gets instantiated on <see cref="Application_Start" />.
        /// </summary>
        //private static IWindsorContainer windsorContainer;
    }
}