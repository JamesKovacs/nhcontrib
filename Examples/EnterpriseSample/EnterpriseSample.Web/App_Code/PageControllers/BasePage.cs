using System;
using System.Web.UI;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Data;

namespace EnterpriseSample.Web
{
    /// <summary>
    /// Summary description for BasePage
    /// </summary>
    public abstract class BasePage : Page
    {
        /// <summary>
        /// Page_Load of the Page Controller pattern.
        /// See http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnpatterns/html/ImpPageController.asp
        /// </summary>
        protected void Page_Load(object sender, EventArgs e) {
            // Do whatever standard code which occurs on every page

            PageLoad();
        }

        protected abstract void PageLoad();

        /// <summary>
        /// Exposes accessor for the <see cref="IDaoFactory" /> used by all pages.
        /// </summary>
        public IDaoFactory DaoFactory {
            get {
                return (IDaoFactory)NHibernateDaoFactory.getInstance();
            }
        }

        // You can expose IAnotherDbDaoFactory here
    }
}
