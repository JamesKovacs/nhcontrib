using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using NHibernate;
using ProjectBase.Data;
using ProjectBase.Utils;

namespace EnterpriseSample.Data
{
    public class HistoricalOrderSummaryDao : IHistoricalOrderSummaryDao
    {
        public List<HistoricalOrderSummary> GetCustomerOrderHistoryFor(Customer customer)
        {
            Check.Require(customer!=null, "customerId may not be null or empty");

            IQuery query = NHibernateSession.GetNamedQuery("GetCustomerOrderHistory")
                .SetString("CustomerID", customer.ID)
                .SetResultTransformer(
                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(
                    typeof (HistoricalOrderSummary).GetConstructors()[0]));

            return query.List<HistoricalOrderSummary>() as List<HistoricalOrderSummary>;
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        private ISession NHibernateSession {
            get {
                return NHibernateSessionManager.Instance.GetSession(typeof(HistoricalOrderSummary));
            }
        }

    }
}
