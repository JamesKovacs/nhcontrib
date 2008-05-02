using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Data;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Data
{
    [TestFixture, Ignore("need refactor")]
    [Category("Database Tests")]
    public class HistoricalOrderSummaryDaoTests : NHibernateTestCase
    {
        [Test]
        public void CanGetCustomerOrderHistory() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            IHistoricalOrderSummaryDao historicalOrderSummaryDao = daoFactory.GetHistoricalOrderSummaryDao();

            List<HistoricalOrderSummary> foundSummaries = historicalOrderSummaryDao.GetCustomerOrderHistoryFor(TestGlobals.TestCustomer);

            Assert.AreEqual(11, foundSummaries.Count);
        }
    }
}
