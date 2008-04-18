using System;
using System.Web.UI;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Data;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class EditCustomer : UserControl {
    //EntityField attribute is added here so that the value of this field will be rememberred through multiple postbacks.
    //you can use EntityFieldNullSafe attribute if the entity might be deleted and you don't have a way to reset this field to null before postback 
    [EntityField] protected Customer customer;
    public event EventHandler Updated;


    public void Bind(Customer c) {
        customer = c;
        //use wwDataBinder to bind some simple properties (optional)
        DataBinder.DataBind(this);

        //here is some advanced binding logic
        grdOrders.DataSource = customer.Orders;
        grdOrders.DataBind();
        grdProductsOrdered.DataSource = ServiceLocator.DaoFactory
                                            .GetHistoricalOrderSummaryDao()
                                            .GetCustomerOrderHistoryFor(c.ID);
        grdProductsOrdered.DataBind();
        Visible = true;
    }

    protected void btnUpdate_OnClick(object sender, EventArgs e) {
        //wwDataBinder is used here to reverse bind the customer, you can totally do it manually like customer.CustomerName = txtCustomerName.Text;
        DataBinder.Unbind(this);
        if (Updated != null)
            Updated(this, new EventArgs());
    }

    protected void btnCancel_OnClick(object sender, EventArgs e) {
        customer = null;
        Visible = false;
    }
}