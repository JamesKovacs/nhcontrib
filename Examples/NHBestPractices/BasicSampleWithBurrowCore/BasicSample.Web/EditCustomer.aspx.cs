using System;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;
using BasicSample.Core.Domain;

public partial class EditCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            DisplayCustomerToEdit();
        }
    }

    private void DisplayCustomerToEdit() {
        IDaoFactory daoFactory = new NHibernateDaoFactory();
        ICustomerDao customerDao = daoFactory.GetCustomerDao();

        // No need to lock the customer since we're just viewing the data
        Customer customerToEdit = customerDao.GetById(Request.QueryString["customerID"], false);

        ShowCustomerDetails(customerToEdit);
        ShowPastOrders(customerToEdit);
        ShowProductsOrdered(customerToEdit);
    }

    private void ShowCustomerDetails(Customer customer) {
        hidCustomerID.Value = customer.ID;
        lblCustomerID.Text = customer.ID;
        txtCompanyName.Text = customer.CompanyName;
        txtContactName.Text = customer.ContactName;
    }

    private void ShowPastOrders(Customer customer) {
        // The Orders collection is lazy-loaded
        grdOrders.DataSource = customer.Orders;
        grdOrders.DataBind();
    }

    private void ShowProductsOrdered(Customer customer) {
        IDaoFactory daoFactory = new NHibernateDaoFactory();
        IHistoricalOrderSummaryDao orderSummaryDao = daoFactory.GetHistoricalOrderSummaryDao();

        grdProductsOrdered.DataSource = orderSummaryDao.GetCustomerOrderHistoryFor(customer.ID);
        grdProductsOrdered.DataBind();
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e) {
        UpdateCustomer();
        Response.Redirect("ListCustomers.aspx?action=updated");
    }

    private void UpdateCustomer() {
        IDaoFactory daoFactory = new NHibernateDaoFactory();
        ICustomerDao customerDao = daoFactory.GetCustomerDao();

        // Now that we're about to update the customer, be sure to lock the entity
        Customer customerToUpdate = customerDao.GetById(hidCustomerID.Value, true);

        // Changes to the customer object will be automatically committed at the end of the HTTP request
        customerToUpdate.CompanyName = txtCompanyName.Text;
        customerToUpdate.ContactName = txtContactName.Text;
    }

    protected void btnCancel_OnClick(object sender, EventArgs e) {
        Response.Redirect("ListCustomers.aspx");
    }
}
