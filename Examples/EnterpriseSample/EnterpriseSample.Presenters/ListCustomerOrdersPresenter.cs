using System;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class ListCustomerOrdersPresenter
    {
        public ListCustomerOrdersPresenter(IListObjectsView<Order> view, ICustomerDao customerDao) {
            Check.Require(view != null, "view may not be null");
            Check.Require(customerDao != null, "customerDao may not be null");

            this.view = view;
            this.customerDao = customerDao;
        }

        public void InitViewWith(string customerId) {
            Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be empty");

            // No need to lock the customer since we're just viewing the data
            Customer customer = customerDao.GetById(customerId, false);

            // Note that the Orders collection is lazy-loaded
            view.ObjectsToList = customer.Orders;
        }

        private IListObjectsView<Order> view;
        private ICustomerDao customerDao;
    }
}
