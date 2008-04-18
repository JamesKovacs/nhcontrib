using System;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Web;
using ProjectBase.Utils;

public partial class Views_EditCustomerView : BaseUserControl, IEditCustomerView
{
    public EventHandler UpdateCompleted;
    public EventHandler UpdateCancelled;
    public EventHandler<CustomerEventArgs> OrdersView;
    
    public void AttachPresenter(EditCustomerPresenter presenter) {
        this.presenter = presenter;
    }

    public Customer Customer {
        set {
            Check.Require(value != null, "Customer may not be null");
            ShowCustomerDetails(value);
            Session[ClientID + "Customer"] = value;
        }
        get { 
            return Session[ClientID + "Customer"] as Customer; 
        }
    }

    private void ShowCustomerDetails(Customer customer) {
        // Instead of using a hidden input, you could also use ViewState...but it's often best
        // to disable ViewState for your entire website
        hidCustomerID.Value = customer.ID;

        lblCustomerID.Text = customer.ID;
        txtCompanyName.Text = customer.CompanyName;
        txtContactName.Text = customer.ContactName;
    }

    /// <summary>
    /// Required by <see cref="IEditCustomerView" /> so that the presenter
    /// can have the view set the values of the customer.
    /// </summary>
    public void UpdateValuesOn(Customer customer) {
        // Changes to the customer object will be automatically committed at the end of the HTTP request
        customer.CompanyName = txtCompanyName.Text;
        customer.ContactName = txtContactName.Text;
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e) {
        presenter.Update(hidCustomerID.Value);

        // The view itself shouldn't be handling any redirects, so simply raise an event letting 
        // whomever know that the action has completed
        if (UpdateCompleted != null) {
            UpdateCompleted(this, EventArgs.Empty);
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e) {
        if (UpdateCancelled != null) {
            UpdateCancelled(this, EventArgs.Empty);
        }
    }

    protected void btnOrdersView_OnClick(object sender, EventArgs e)
    {
        if (OrdersView != null)
            OrdersView(this, new CustomerEventArgs(this.Customer));
    }

    private EditCustomerPresenter presenter;
}