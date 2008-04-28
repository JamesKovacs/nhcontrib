using System;
using System.Collections.Generic;
using System.Web.UI;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
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
public partial class EditCustomer : Page, IEditCustomerView
{
    private EditCustomerPresenter presenter;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BurrowFramework burrow = new BurrowFramework();
            burrow.CurrentConversation.SpanWithPostBacks();
        }

        InitEditCustomerView();
    }

    private void InitEditCustomerView()
    {
        presenter = new EditCustomerPresenter(this);
        ctrlEditCustomerView.AttachPresenter(presenter);

        // Listen for events coming from the view
        ctrlEditCustomerView.UpdateCompleted += HandleUpdateCompleted;
        ctrlEditCustomerView.UpdateCancelled += HandleUpdateCancelled;

        if (!IsPostBack)
        {
            presenter.InitViewWith(CustomerId);
        }
    }

    private void HandleUpdateCancelled(object sender, EventArgs e)
    {
        BurrowFramework burrow = new BurrowFramework();
        burrow.CurrentConversation.FinishSpan();
        Response.Redirect("ListCustomers.aspx", false);
    }

    private void HandleUpdateCompleted(object sender, EventArgs e)
    {
        // PageMethods is perfect for managing strongly-typed redirects...
        // definitely use it with your web apps instead of the following.
        BurrowFramework burrow = new BurrowFramework();
        burrow.CurrentConversation.FinishSpan();
        Response.Redirect("ListCustomers.aspx?action=updated", false);
    }

    private string CustomerId
    {
        get
        {
            // This is a terrible way to manage querystring data...there's no enforcement
            // of business rules whatsoever.  Instead, I highly recommend using 
            // PageMethods found at http://metasapiens.com/PageMethods
            return Request.QueryString["customerID"];
        }
    }

    #region IEditCustomerView Members

    public Customer Customer
    {
        set { ctrlEditCustomerView.Customer = value; }
    }

    public void UpdateValuesOn(Customer customer)
    {
        ctrlEditCustomerView.UpdateValuesOn(customer);
    }

    public IList<Order> Orders
    {
        set { ctrlListOrdersView.Orders = value; }
    }

    public IList<HistoricalOrderSummary> HistoricalOrders
    {
        set { ctrlListHistoricalOrderSummariesView.HistoricalOrderSummary = value; }
    }

    #endregion
}
