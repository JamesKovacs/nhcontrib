using System;
using System.Web.UI;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Data;
using NHibernate.Burrow;

/// <summary>
/// This page give a simple sample of spanning conversation 
/// </summary>
public partial class PlaceOrder : Page {
    private IDaoFactory daoFactory = new NHibernateDaoFactory();
    private BurrowFramework facade = new BurrowFramework();

    /// <summary>
    /// Store the placing order in the Conversation.Items so that it has the same life span as the Conversation
    /// </summary>
    public Order placingOrder {
        get { return (Order) facade.CurrentConversation.Items["placingOrder"]; }
        set { facade.CurrentConversation.Items["placingOrder"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            ddlCustomers.DataSource = daoFactory.GetCustomerDao().GetAll();
            ddlCustomers.DataBind();
        }
    }

    protected void Step1(object sender, EventArgs e) {
        facade.CurrentConversation.SpanWithPostBacks(); //start a Conversation that span over postbacks
        Customer selectedCustomer = daoFactory.GetCustomerDao().GetById(ddlCustomers.SelectedValue, false);
        placingOrder =  new Order(selectedCustomer) ;
        phSelectCustomer.Visible = false;
        phEnterShipToName.Visible = true;
    }

    protected void btnStep2(object sender, EventArgs e) {
        placingOrder.ShipToName = tbShipToName.Text;
        phEnterShipToName.Visible = false;
        phConfirm.Visible = true;
        lCustomer.Text = placingOrder.OrderedBy.ToString();
        lShipTo.Text = placingOrder.ShipToName;
    }

    protected void Cancel(object sender, EventArgs e) {
        facade.CurrentConversation.GiveUp(); // give up the current spanning conversation
        phEnterShipToName.Visible = false;
        StartOver();
    }

    private void StartOver() {
        phSelectCustomer.Visible = true;
    }

    protected void Finish(object sender, EventArgs e) {
        placingOrder.OrderDate = DateTime.Now;
        daoFactory.GetOrderDao().Save(placingOrder);
        facade.CurrentConversation.FinishSpan(); //finish up the current spanning conversation
        placingOrder = null; //reset the placing order to null after conversation is done is a good practice
        phConfirm.Visible = false;
        StartOver();
    }
}