using System;
using System.Web.UI;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Data;

/// <summary>
/// This could alternatively be hooked up via MVP; for simplicity of the sample, it's not.  See EditCustomer.aspx for a good example of MVP.
/// </summary>
public partial class ListSuppliers : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            DisplayAllSuppliers();
        }
    }

    private void DisplayAllSuppliers() {
        ISupplierDao supplierDao = new NHibernateDaoFactory().GetSupplierDao();
        grdSuppliers.DataSource = supplierDao.GetAll();
        grdSuppliers.DataBind();
    }
}
