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

        private readonly EditCustomerModel model;

        public EditCustomerPresenter(IEditCustomerView view)
        {
            Check.Require(view != null, "view may not be null");
            this.view = view;

            BurrowFramework burrow = new BurrowFramework();

            //Save model in current conversation.
            object mod;
            if (!burrow.CurrentConversation.Items.TryGetValue("EditCustomerModel", out mod))
            {
                model = new EditCustomerModel();
                burrow.CurrentConversation.Items.Add("EditCustomerModel",model);
            }
            else
                model = mod as EditCustomerModel;
        }

        public void InitViewWith(string customerId) {
            Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be empty");

            model.setCustomer (customerId);
            view.Customer = model.Customer;
        }

        public void Update()
        {
            Check.Require(model.Customer != null, "customerId may not be empty");

            view.UpdateValuesOn(model.Customer);

            model.Update();
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
