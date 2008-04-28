using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class ListCustomersPresenter : PresenterBase
    {
        private readonly IListCustomersView view;

        public ListCustomersPresenter(IListCustomersView view)
        {
            Check.Require(view != null, "view may not be null");
            this.view = view;
        }

        public void InitView() {
            ICustomerDao customerDao = DaoFactory.GetCustomerDao();
            view.Customers = customerDao.GetAll();
        }
    }
}
