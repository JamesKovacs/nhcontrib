using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Tests.Data.DaoTestDoubles;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Presenters
{
    /// <summary>
    /// Is this class name too long?  Perhaps, but I don't think so.  It accurately describes what 
    /// the class is doing while stilling being concise.  "LstHistOrdrSumPresTests" would certainly 
    /// shorten it but would make it a bit less clear.
    /// </summary>
    [TestFixture]
    public class ListHistoricalOrderSummariesPresenterTests
    {
        [Test]
        public void TestInitView() {
            ListHistoricalOrderSummaryViewStub view = new ListHistoricalOrderSummaryViewStub();
            ListHistoricalOrderSummariesPresenter presenter = new ListHistoricalOrderSummariesPresenter(view);
                //new MockHistoricalOrderSummaryDaoFactory().CreateMockHistoricalOrderSummariesDao());
            presenter.InitViewWith(TestGlobals.TestCustomer);

            Assert.IsNotNull(view.ObjectsToList);
            Assert.AreEqual(4, view.ObjectsToList.Count);
        }

        private class ListHistoricalOrderSummaryViewStub : IListObjectsView<HistoricalOrderSummary>
        {
            public IList<HistoricalOrderSummary> ObjectsToList {
                set { summaries = value; }
                // Not required by IListObjectsView, but useful for unit test verfication
                get { return summaries; }
            }

            private IList<HistoricalOrderSummary> summaries;
        }
    }
}
