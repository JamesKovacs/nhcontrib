using System;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;
using BasicSample.Core.Domain;

public partial class AddCustomer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            lblMessage.Text = "Use this form to add a new customer.";
        }
    }

    protected void btnAdd_OnClick(object sender, EventArgs e) {
        if (txtCustomerID.Text.Trim().Length == 5) {
            Customer newCustomer = new Customer(txtCompanyName.Text);
            newCustomer.SetAssignedIdTo(txtCustomerID.Text);
            newCustomer.ContactName = txtContactName.Text;

            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();

            if (!IsDuplicateOfExisting(newCustomer, customerDao)) {
                customerDao.Save(newCustomer);
                Response.Redirect("ListCustomers.aspx?action=added");
            }
            else {
                lblMessage.Text =
                    "<span style=\"color:red\">The ID you provided is already in use.</span><br />Please change the ID and try again.";
            }
        }
        else {
            lblMessage.Text =
                "<span style=\"color:red\">The ID you provide must be exactly 5 characters long.</span><br />Please change the ID and try again.";
        }
    }

    /// <summary>
    /// Checks if a customer already exists with the same customer ID.
    /// </summary>
    private bool IsDuplicateOfExisting(Customer newCustomer, ICustomerDao customerDao) {
        // Whenever possible, I *really* don't like using assigned IDs.  I think they 
        // should only be used when working with a legacy database.  Among other ugliness, 
        // assigned IDs force us to try/catch when checking for duplicates because NHibernate 
        // will throw an ObjectNotFoundException if no entity with the provided ID is found.
        // Consequently, we also have to have a reference to the NHibernate assembly from within
        // our business object.
        // To overcome these drawbacks, I'd recommend adding a DoesEntityExist(string assignedId) 
        // method to the DAO to check for the existence of entities by its assigned ID.  This would remove the
        // ugly try/catch from this method and it would also remove the local dependency on the NHibernate
        // assembly.  I've chosen not to go ahead and do this because my assumption is that
        // the use of assigned IDs will be the exception rather than the norm...so I want to keep
        // the generic DAO as clean as possible for the example demo.
        try {
            Customer duplicateCustomer = customerDao.GetById(newCustomer.ID, false);
            return duplicateCustomer != null;
        }
        // Only catch ObjectNotFoundException, throw everything else.
        catch (NHibernate.ObjectNotFoundException) {
            // Since the duplicate we were looking for wasn't found, then, through difficult 
            // logical deduction, this object isn't a duplicat
            return false;
        }
    }

    protected void btnCancel_OnClick(object sender, EventArgs e) {
        Response.Redirect("ListCustomers.aspx");
    }
}
