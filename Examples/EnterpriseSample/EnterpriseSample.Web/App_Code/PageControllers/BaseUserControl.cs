using System.Web.UI;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Data;

namespace EnterpriseSample.Web
{
    /// <summary>
    /// Summary description for BaseControl
    /// </summary>
    public abstract class BaseUserControl : UserControl
    {
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