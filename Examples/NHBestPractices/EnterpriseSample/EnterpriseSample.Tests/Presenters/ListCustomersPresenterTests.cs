using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Presenters
{
    [TestFixture]
    public class ListCustomersPresenterTests
    {
        [Test]
        public void TestInitView() {
            ListCustomersViewStub view = new ListCustomersViewStub();
            ListCustomersPresenter presenter = new ListCustomersPresenter(view);
                //new MockCustomerDaoFactory().CreateMockCustomerDao());
            presenter.InitView();

            Assert.IsNotNull(view.Customers);
            Assert.AreEqual(3, view.Customers.Count);
        }

        private class ListCustomersViewStub : IListCustomersView
        {
            public IList<Customer> Customers {
                set { customers = value; }
                // Not required by IListCustomersView, but useful for unit test verfication
                get { return customers; }
            }

            private IList<Customer> customers;
        }
				
    }
}
