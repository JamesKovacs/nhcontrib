using System;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class ListCustomerOrdersPresenter : PresenterBase
    {
        public ListCustomerOrdersPresenter(IListObjectsView<Order> view) {
            Check.Require(view != null, "view may not be null");

            this.view = view;
            this.customerDao = DaoFactory.GetCustomerDao();
        }

        public void InitViewWith(Customer customer) {
            Check.Require(customer!=null, "customer may not be null");

            // Note that the Orders collection is lazy-loaded
            view.ObjectsToList = customer.Orders;
        }

        private IListObjectsView<Order> view;
        private ICustomerDao customerDao;
    }
}
