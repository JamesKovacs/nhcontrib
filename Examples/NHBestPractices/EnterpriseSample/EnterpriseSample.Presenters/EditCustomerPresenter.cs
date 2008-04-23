using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Data;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class EditCustomerPresenter
    {
        public EditCustomerPresenter(IEditCustomerView view, ICustomerDao customerDao) {
            Check.Require(view != null, "view may not be null");
            Check.Require(customerDao != null, "customerDao may not be null");

            this.view = view;
            this.customerDao = customerDao;
        }

        public void InitViewWith(string customerId) {
            Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be empty");

            // No need to lock the customer since we're just viewing the data
            view.Customer = customerDao.GetById(customerId, false);
        }

        public void Update(Customer customer)
        {
            Check.Require(customer!=null, "customerId may not be empty");

            view.UpdateValuesOn(customer);
            
            customerDao.SaveOrUpdate(customer);
        }

        private IEditCustomerView view;
        private ICustomerDao customerDao;
    }
}
