using System;
using System.Web.UI;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Data;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Web;
using NHibernate;

/// <summary>
/// This could alternatively be hooked up via MVP; for simplicity of the sample, it's not.  See EditCustomer.aspx for a good example of MVP.
/// </summary>
public partial class AddCustomer : Page, IAddCustomerView
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            lblMessage.Text = "Use this form to add a new customer.";
        }
    }

    protected void btnAdd_OnClick(object sender, EventArgs e) {
        AddCustomerPresenter presenter = new AddCustomerPresenter(this);
        if (presenter.Create())
            Response.Redirect("ListCustomers.aspx?action=added", false);
    }

    protected void btnCancel_OnClick(object sender, EventArgs e) {
        Response.Redirect("ListCustomers.aspx", false);
    }

    #region IAddCustomerView Members

    public string Message
    {
        set { lblMessage.Text = value; }
    }

    public Customer Customer
    {
        get
        {
            Customer newCustomer = new Customer(txtCompanyName.Text);
            newCustomer.SetAssignedIdTo(txtCustomerID.Text);
            newCustomer.ContactName = txtContactName.Text;

            return newCustomer;
        }
    }

    #endregion
}
