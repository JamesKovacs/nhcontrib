using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Models;
using EnterpriseSample.Presenters.ViewInterfaces;
using NHibernate.Burrow;
using ProjectBase.Data;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class EditCustomerPresenter : PresenterBase
    {
        private readonly IEditCustomerView view;
        private readonly ICustomerDao customerDao;

        private readonly EditCustomerModel model;

        public EditCustomerPresenter(IEditCustomerView view)
        {
            Check.Require(view != null, "view may not be null");
            this.view = view;

            customerDao = DaoFactory.GetCustomerDao();

            BurrowFramework burrow = new BurrowFramework();

            //Save model in current conversation.
            if (!burrow.CurrentConversation.Items.ContainsKey("EditCustomerModel"))
                burrow.CurrentConversation.Items.Add("EditCustomerModel", new EditCustomerModel());
            model = burrow.CurrentConversation.Items["EditCustomerModel"] as EditCustomerModel;

        }

        public void InitViewWith(string customerId) {
            Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be empty");

            // No need to lock the customer since we're just viewing the data
            model.Customer = customerDao.GetById(customerId, false);
            
            view.Customer = model.Customer;
        }

        public void Update()
        {
            Check.Require(model.Customer != null, "customerId may not be empty");

            view.UpdateValuesOn(model.Customer);
            customerDao.SaveOrUpdate(model.Customer);
        }

        public void ShowOrders()
        {
            view.Orders = model.Customer.Orders;
        }

        public void ShowHistoricalOrders()
        {
            IHistoricalOrderSummaryDao dao = DaoFactory.GetHistoricalOrderSummaryDao();
            view.HistoricalOrders = dao.GetCustomerOrderHistoryFor(model.Customer);
        }
    }
}
