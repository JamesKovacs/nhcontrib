using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class ListCustomersPresenter
    {
        public ListCustomersPresenter(IListCustomersView view, ICustomerDao customerDao) {
            Check.Require(view != null, "view may not be null");
            Check.Require(customerDao != null, "customerDao may not be null");

            this.view = view;
            this.customerDao = customerDao;
        }

        public void InitView() {
            view.Customers = customerDao.GetAll();
        }

        private IListCustomersView view;
        private ICustomerDao customerDao;
    }
}
