using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Core.Utils;
using NHibernate;
using NHibernate.Burrow;

namespace BasicSample.Data
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
                return Facade.GetSession();
            }
        }
    }
}
