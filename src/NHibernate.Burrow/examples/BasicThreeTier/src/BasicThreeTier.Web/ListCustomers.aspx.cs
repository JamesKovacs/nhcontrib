using System;
using System.Web.UI.WebControls;
using BasicThreeTier.Core.Dao;
using BasicThreeTier.Core.Domain;

public partial class ListCustomers : System.Web.UI.Page
{
    protected void lbt_SelectCommand(object sender, CommandEventArgs e) {
        EditCustomer1.Bind(new CustomerDAO().Get(int.Parse(e.CommandArgument.ToString())));
    } 
    
    protected void EditCustomer_Updated(object sender, EventArgs e) {
        grdCustomers.DataBind();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Customer c = new Customer("new Customer");
        new CustomerDAO().Save(c);
        grdCustomers.DataBind();

    }
}
