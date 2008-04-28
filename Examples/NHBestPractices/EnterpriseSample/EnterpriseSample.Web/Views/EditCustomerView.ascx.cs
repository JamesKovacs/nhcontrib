using System;
using System.Web.UI;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

public partial class Views_EditCustomerView : UserControl
{
    public EventHandler UpdateCompleted;
    public EventHandler UpdateCancelled;
    
    public void AttachPresenter(EditCustomerPresenter presenter) {
        this.presenter = presenter;
    }

    public Customer Customer {
        set {
            Check.Require(value != null, "Customer may not be null");
            ShowCustomerDetails(value);
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
        presenter.Update();

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
            presenter.ShowOrders();
    }

    protected void btnHistoricalOrdersView_OnClick(object sender, EventArgs e)
    {
            presenter.ShowHistoricalOrders();
    }
    

    private EditCustomerPresenter presenter;
}