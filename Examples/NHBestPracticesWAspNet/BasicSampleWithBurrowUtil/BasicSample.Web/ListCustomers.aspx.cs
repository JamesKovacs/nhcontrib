using System;
using System.Web.UI.WebControls;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;

public partial class ListCustomers : System.Web.UI.Page
{
    protected void lbt_SelectCommand(object sender, CommandEventArgs e) {
        EditCustomer1.Bind(ServiceLocator.DaoFactory.GetCustomerDao().Get(e.CommandArgument.ToString()));
    } 
    
    protected void EditCustomer_Updated(object sender, EventArgs e) {
        grdEmployees.DataBind();
    }
}
