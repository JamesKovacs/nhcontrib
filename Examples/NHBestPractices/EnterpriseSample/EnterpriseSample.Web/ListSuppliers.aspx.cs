using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Web;

/// <summary>
/// This could alternatively be hooked up via MVP; for simplicity of the sample, it's not.  See EditCustomer.aspx for a good example of MVP.
/// </summary>
public partial class ListSuppliers : BasePage
{
    protected override void PageLoad() {
        if (!IsPostBack) {
            DisplayAllSuppliers();
        }
    }

    private void DisplayAllSuppliers() {
        ISupplierDao supplierDao = DaoFactory.GetSupplierDao();

        grdSuppliers.DataSource = supplierDao.GetAll();
        grdSuppliers.DataBind();
    }
}
