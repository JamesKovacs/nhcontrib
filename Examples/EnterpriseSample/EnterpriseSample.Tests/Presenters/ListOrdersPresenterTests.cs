using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Tests.Data.DaoTestDoubles;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Presenters
{
    [TestFixture]
    public class ListOrdersPresenterTests
    {
        [Test]
        public void TestInitView() {
            ListOrdersViewStub view = new ListOrdersViewStub();
            ListCustomerOrdersPresenter presenter = new ListCustomerOrdersPresenter(view,
                new MockCustomerDaoFactory().CreateMockCustomerDao());
            presenter.InitViewWith(TestGlobals.TestCustomer);

            Assert.IsNotNull(view.ObjectsToList);
            Assert.AreEqual(3, view.ObjectsToList.Count);
        }

        private class ListOrdersViewStub : IListObjectsView<Order>
        {
            public IList<Order> ObjectsToList {
                set { orders = value; }
                // Not required by IListObjectsView, but useful for unit test verfication
                get { return orders; }
            }

            private IList<Order> orders;
        }
    }
}
