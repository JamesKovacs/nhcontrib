using System;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Web;
using NHibernate.Burrow;

/// <summary>
/// In this page, as opposed to ListCustomers.aspx, this acts as a view-initializer wherein
/// it 1) wires up views to their respective presenters and 2) handles navigational flow after 
/// receiving events from the views.  It could be argued that the views are too minimal and 
/// that it would be OK to combine ctrlListOrdersView and ctrlListHistoricalOrderSummariesView, 
/// but it serves well to demonstrate using multiple views on the same page.  If you only plan 
/// on having one view on a page, take a look at ListCustomers.aspx.
/// </summary>
public partial class EditCustomer : BasePage
{
    protected override void PageLoad() {
        if (!IsPostBack)
        {
            BurrowFramework burrow = new BurrowFramework();
            burrow.CurrentConversation.SpanWithPostBacks();
        }

        InitEditCustomerView();
        //InitListOrdersView();
        //InitListHistoricalOrderSummariesView();
    }

    private void InitEditCustomerView() {
        EditCustomerPresenter presenter = new EditCustomerPresenter(ctrlEditCustomerView, DaoFactory.GetCustomerDao());
        ctrlEditCustomerView.AttachPresenter(presenter);
        // Listen for events coming from the view
        ctrlEditCustomerView.UpdateCompleted += HandleUpdateCompleted;
        ctrlEditCustomerView.UpdateCancelled += HandleUpdateCancelled;
        ctrlEditCustomerView.OrdersView += HandleOrdersView;
        ctrlEditCustomerView.HistoricalOrdersView += HandleHistoricalOrdersView;

        if (!IsPostBack) {
            presenter.InitViewWith(CustomerId);
        }
    }

    private void HandleUpdateCancelled(object sender, EventArgs e) {
        BurrowFramework burrow = new BurrowFramework();
        burrow.CurrentConversation.FinishSpan();
        Response.Redirect("ListCustomers.aspx", false);
    }

    private void HandleUpdateCompleted(object sender, EventArgs e) {
        // PageMethods is perfect for managing strongly-typed redirects...
        // definitely use it with your web apps instead of the following.
        BurrowFramework burrow = new BurrowFramework();
        burrow.CurrentConversation.FinishSpan();
        Response.Redirect("ListCustomers.aspx?action=updated", false);
    }

    private void HandleOrdersView(object sender, CustomerEventArgs e)
    {
        InitListOrdersView(e.Customer);
    }

    private void HandleHistoricalOrdersView(object sender, CustomerEventArgs e)
    {
        InitListHistoricalOrderSummariesView(e.Customer);
    }

    private void InitListOrdersView(Customer customer)
    {
        ListCustomerOrdersPresenter presenter = new ListCustomerOrdersPresenter(ctrlListOrdersView,
            DaoFactory.GetCustomerDao());
        presenter.InitViewWith(customer);
    }

    private void InitListHistoricalOrderSummariesView(Customer customer)
    {
        ListHistoricalOrderSummariesPresenter presenter = new ListHistoricalOrderSummariesPresenter(
            ctrlListHistoricalOrderSummariesView, DaoFactory.GetHistoricalOrderSummaryDao());

        presenter.InitViewWith(customer);
    }

    private string CustomerId {
        get {
            // This is a terrible way to manage querystring data...there's no enforcement
            // of business rules whatsoever.  Instead, I highly recommend using 
            // PageMethods found at http://metasapiens.com/PageMethods
            return Request.QueryString["customerID"];
        }
    }
}
