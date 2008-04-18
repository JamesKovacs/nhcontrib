using System.Web.Services;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Data;

namespace EnterpriseSample.Web
{
    /// <summary>
    /// Summary description for BaseWebService
    /// </summary>
    public abstract class BaseWebService : WebService
    {
        /// <summary>
        /// Exposes accessor for the <see cref="IDaoFactory" /> used by all pages.
        /// </summary>
        public IDaoFactory DaoFactory {
            get {
                return (IDaoFactory)NHibernateDaoFactory.getInstance(); ;
            }
        }
    }
}