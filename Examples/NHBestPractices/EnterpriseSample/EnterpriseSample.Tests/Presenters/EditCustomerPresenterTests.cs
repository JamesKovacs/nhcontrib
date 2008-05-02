using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Tests.Data;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Presenters
{
    [TestFixture]
    public class EditCustomerPresenterTests : NHibernateTestCase
    {
        [Test]
        public void CanInitView() {
            EditCustomerViewStub view = new EditCustomerViewStub();
            EditCustomerPresenter presenter = new EditCustomerPresenter(view);

            view.AttachPresenter(presenter);
            presenter.InitViewWith(TestGlobals.TestCustomer.ID);

            Assert.AreEqual(TestGlobals.TestCustomer.ID, view.Customer.ID);
            Assert.AreEqual(TestGlobals.TestCustomer.CompanyName, view.Customer.CompanyName);
            Assert.AreEqual(TestGlobals.TestCustomer.ContactName, view.Customer.ContactName);
        }

        [Test, Ignore("Test case don't passed yet")]
        public void CanUpdateCustomer() {
            EditCustomerViewStub view = new EditCustomerViewStub();
            EditCustomerPresenter presenter =new EditCustomerPresenter(view); 

            presenter.InitViewWith(TestGlobals.TestCustomer.ID);
            presenter.Update();
        }

        private class EditCustomerViewStub : IEditCustomerView
        {
            public Customer Customer {
                set { customer = value; }
                get { return customer; }
            }

            /// <summary>
            /// Although testing this method is rather useless, the unit test itself
            /// provides good guidance for determining the communications protocol
            /// between the MVP components.
            /// </summary>
            public void UpdateValuesOn(Customer customer) {}

            public IList<Order> Orders { set { } }

            public IList<HistoricalOrderSummary> HistoricalOrders { set { } }

            public void AttachPresenter(EditCustomerPresenter presenter) {}

            private Customer customer;
        }
    }
}
