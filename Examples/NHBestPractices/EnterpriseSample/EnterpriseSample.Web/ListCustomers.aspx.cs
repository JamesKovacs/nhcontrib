using System;
using System.Collections.Generic;
using System.Web.UI;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Web;

/// <summary>
/// In this page, as opposed to within EditCustomer.aspx, the view and the ASPX-view-initializer
/// are one and the same.
/// </summary>
public partial class ListCustomers : Page, IListCustomersView
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DisplayMessage();
            InitView();
        }
    }

    private void InitView() {
        ListCustomersPresenter presenter = new ListCustomersPresenter(this);
        presenter.InitView();
    }

    private void DisplayMessage() {
        if (Request.QueryString["action"] == "updated") {
            lblMessage.Text = "The customer was successfully updated.";
        }
        else if (Request.QueryString["action"] == "added") {
            lblMessage.Text = "The customer was successfully added.";
        }
        else {
            lblMessage.Text = "Click a customer's ID to edit the customer.";
        }
    }

    public IList<Customer> Customers {
        set { 
            grdEmployees.DataSource = value;
            grdEmployees.DataBind();
        }
    }
}
