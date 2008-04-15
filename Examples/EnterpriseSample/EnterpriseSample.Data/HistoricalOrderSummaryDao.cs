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
        public List<HistoricalOrderSummary> GetCustomerOrderHistoryFor(string customerId) {
            Check.Require(! string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            IQuery query = NHibernateSession.GetNamedQuery("GetCustomerOrderHistory")
                .SetString("CustomerID", customerId)
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
                return NHibernateSessionManager.Instance.GetSession();
            }
        }

    }
}
