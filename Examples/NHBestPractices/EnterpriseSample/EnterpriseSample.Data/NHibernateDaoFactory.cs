using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using ProjectBase.Data;
using ProjectBase.Utils;

namespace EnterpriseSample.Data
{
    /// <summary>
    /// Exposes access to NHibernate DAO classes.  Motivation for this DAO
    /// framework can be found at http://www.hibernate.org/328.html.
    /// </summary>
    public class NHibernateDaoFactory : IDaoFactory
    {
        public ICustomerDao GetCustomerDao() {
            return new CustomerDao();
        }

        public IHistoricalOrderSummaryDao GetHistoricalOrderSummaryDao() {
            return new HistoricalOrderSummaryDao();
        }

        public IOrderDao GetOrderDao() {
            return new OrderDao();
        }

        public ISupplierDao GetSupplierDao() {
            return new SupplierDao();
        }
    }

    #region Inline DAO implementations

    /// <summary>
    /// Concrete DAO for accessing instances of <see cref="Customer" /> from DB.
    /// This should be extracted into its own class-file if it needs to extend the
    /// inherited DAO functionality.
    /// </summary>
    public class CustomerDao : AbstractNHibernateDao<Customer, string>, ICustomerDao
    { }

    public class SupplierDao : AbstractNHibernateDao<Supplier, long>, ISupplierDao
    { }

    #endregion
}
