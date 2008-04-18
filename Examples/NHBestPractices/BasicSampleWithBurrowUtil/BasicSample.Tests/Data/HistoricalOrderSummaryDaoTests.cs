using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Data;
using NUnit.Framework;

namespace BasicSample.Tests.Data
{
    [TestFixture]
    [Category("Database Tests")]
    public class HistoricalOrderSummaryDaoTests :NHibernateTestCase
    {
        [Test]
        public void CanGetCustomerOrderHistory() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            IHistoricalOrderSummaryDao historicalOrderSummaryDao = daoFactory.GetHistoricalOrderSummaryDao();
            List<HistoricalOrderSummary> foundSummaries = historicalOrderSummaryDao.GetCustomerOrderHistoryFor(TestGlobals.TestCustomer.ID);
            Assert.AreEqual(11, foundSummaries.Count);
        }
    }
}
