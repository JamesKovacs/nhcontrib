using System;
using System.Web.UI;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Data;
using NHibernate.Burrow;
using NHibernate.Burrow.WebUtil.Attributes;

/// <summary>
/// This page give a simple sample of spanning conversation 
/// </summary>
public partial class PlaceOrder : Page {
    private IDaoFactory daoFactory = ServiceLocator.DaoFactory;
    private Facade facade = new Facade();

    /// <summary>
    /// using a ConversationalField attribute so that it has the same life span as the Conversation
    /// </summary>
    [ConversationalField]
    protected Order placingOrder;

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            FetchCustomerDropdown();
        }
    }

    private void FetchCustomerDropdown() {
        ddlCustomers.DataSource = daoFactory.GetCustomerDao().FindAll();
        ddlCustomers.DataBind();
    }

    /// <summary>
    /// Step 1 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelectCustomer_Click(object sender, EventArgs e) {
        facade.CurrentConversation.SpanWithPostBacks(); //start a Conversation that span over postbacks
        Customer selectedCustomer = daoFactory.GetCustomerDao().Get(ddlCustomers.SelectedValue);
        placingOrder =  new Order(selectedCustomer);

        //display the next step
        phSelectCustomer.Visible = false;
        phEnterShipToName.Visible = true;
        lMessage.Text = string.Empty;
    }

    /// <summary>
    /// Step 2
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEnterShipTo_Click(object sender, EventArgs e) {
        placingOrder.ShipToName = tbShipToName.Text;

        //display the next step
        phEnterShipToName.Visible = false;
        phConfirm.Visible = true;
        lCustomer.Text = placingOrder.OrderedBy.ToString();
        lShipTo.Text = placingOrder.ShipToName;
    }

    /// <summary>
    /// user cancel the transaction
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e) {
        facade.CurrentConversation.GiveUp(); // give up the current spanning conversation
        StartOver("Order Canceled!");
    }

    /// <summary>
    /// final step
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConfirm_Click(object sender, EventArgs e) {
        placingOrder.OrderDate = DateTime.Now; //Set the order date
        daoFactory.GetOrderDao().Save(placingOrder); //persists the order
        facade.CurrentConversation.FinishSpan(); //finish the current spanning conversation (this method means the conversation is successfully finished)
        StartOver("Order Placed!");
    }

    private void StartOver(string msg)
    {
        placingOrder = null; //it's a good practic to reset the conversational data (data that has the same life span as conversation) to null after conversation is finished, but it's not mandatory
        lMessage.Text = msg + " Let's start over!";
        phEnterShipToName.Visible = false;
        phConfirm.Visible = false;
        phSelectCustomer.Visible = true;
    }


}