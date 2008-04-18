using System;
using System.Web.UI;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;

public partial class ListSuppliers : Page
{
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            DisplayAllSuppliers();
        }
    }

    private void DisplayAllSuppliers() {
        IDaoFactory daoFactory = new NHibernateDaoFactory();
        ISupplierDao supplierDao = daoFactory.GetSupplierDao();

        grdSuppliers.DataSource = supplierDao.GetAll();
        grdSuppliers.DataBind();
    }
}
