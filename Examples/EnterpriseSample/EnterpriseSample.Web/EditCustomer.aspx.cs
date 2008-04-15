using System;
using EnterpriseSample.Presenters;
using EnterpriseSample.Web;

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
        InitEditCustomerView();
        InitListOrdersView();
        InitListHistoricalOrderSummariesView();
    }

    private void InitEditCustomerView() {
        EditCustomerPresenter presenter = new EditCustomerPresenter(ctrlEditCustomerView, DaoFactory.GetCustomerDao());
        ctrlEditCustomerView.AttachPresenter(presenter);
        // Listen for events coming from the view
        ctrlEditCustomerView.UpdateCompleted += new EventHandler(HandleUpdateCompleted);
        ctrlEditCustomerView.UpdateCancelled += new EventHandler(HandleUpdateCancelled);

        if (!IsPostBack) {
            presenter.InitViewWith(CustomerId);
        }
    }

    private void HandleUpdateCancelled(object sender, EventArgs e) {
        Response.Redirect("ListCustomers.aspx");
    }

    private void HandleUpdateCompleted(object sender, EventArgs e) {
        // PageMethods is perfect for managing strongly-typed redirects...
        // definitely use it with your web apps instead of the following.
        Response.Redirect("ListCustomers.aspx?action=updated");

    }

    private void InitListOrdersView() {
        ListCustomerOrdersPresenter presenter = new ListCustomerOrdersPresenter(ctrlListOrdersView,
            DaoFactory.GetCustomerDao());

        if (!IsPostBack) {
            presenter.InitViewWith(CustomerId);
        }
    }

    private void InitListHistoricalOrderSummariesView() {
        ListHistoricalOrderSummariesPresenter presenter = new ListHistoricalOrderSummariesPresenter(
            ctrlListHistoricalOrderSummariesView, DaoFactory.GetHistoricalOrderSummaryDao());

        if (!IsPostBack) {
            presenter.InitViewWith(CustomerId);
        }
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
