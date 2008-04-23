using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Tests.Data.DaoTestDoubles;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Presenters
{
    [TestFixture]
    public class EditCustomerPresenterTests
    {
        [Test]
        public void CanInitView() {
            EditCustomerViewStub view = new EditCustomerViewStub();
            EditCustomerPresenter presenter = new EditCustomerPresenter(view, 
                                new MockCustomerDaoFactory().CreateMockCustomerDao());
            view.AttachPresenter(presenter);
            presenter.InitViewWith(TestGlobals.TestCustomer.ID);

            Assert.AreEqual(TestGlobals.TestCustomer.ID, view.Customer.ID);
        }

        [Test]
        public void CanUpdateCustomer() {
            EditCustomerViewStub view = new EditCustomerViewStub();
            EditCustomerPresenter presenter = new EditCustomerPresenter(view,
                                new MockCustomerDaoFactory().CreateMockCustomerDao());

            presenter.Update(TestGlobals.TestCustomer);
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

            public void AttachPresenter(EditCustomerPresenter presenter) {}

            private Customer customer;
        }
    }
}
