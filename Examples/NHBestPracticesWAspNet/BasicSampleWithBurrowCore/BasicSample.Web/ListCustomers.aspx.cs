using System;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;

public partial class ListCustomers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            DisplayAllCustomers();
            DisplayMessage();
        }
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

    private void DisplayAllCustomers() {
        IDaoFactory daoFactory = new NHibernateDaoFactory();
        ICustomerDao customerDao = daoFactory.GetCustomerDao();

        grdEmployees.DataSource = customerDao.GetAll();
        grdEmployees.DataBind();
    }
}
