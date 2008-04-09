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
    private Facade facade = new Facade();

    /// <summary>
    /// Store the placing order in the <see cref="ConversationalData{T}"/> container so that it has the same life span as the Conversation
    /// </summary>
    public ConversationalData<Order> placingOrder {
        get { return (ConversationalData<Order>) ViewState["placingOrder"]; }
        set { ViewState["placingOrder"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            ddlCustomers.DataSource = daoFactory.GetCustomerDao().GetAll();
            ddlCustomers.DataBind();
        }
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e) {
        facade.CurrentConversation.SpanWithPostBacks(); //start a Conversation that span over postbacks
        Customer selectedCustomer = daoFactory.GetCustomerDao().GetById(ddlCustomers.SelectedValue, false);
        placingOrder = new ConversationalData<Order>( new Order(selectedCustomer) );

        phSelectCustomer.Visible = false;
        phEnterShipToName.Visible = true;
    }

    protected void btnEnterShipTo_Click(object sender, EventArgs e) {
        placingOrder.Value.ShipToName = tbShipToName.Text;


        phEnterShipToName.Visible = false;
        phConfirm.Visible = true;

        lCustomer.Text = placingOrder.Value.OrderedBy.ToString();
        lShipTo.Text = placingOrder.Value.ShipToName;
    }

    protected void btnCancel_Click(object sender, EventArgs e) {
        facade.CurrentConversation.GiveUp(); // give up the current spanning conversation
        phEnterShipToName.Visible = false;
        StartOver();
    }

    private void StartOver() {
        
        phSelectCustomer.Visible = true;
    }

    protected void btnConfirm_Click(object sender, EventArgs e) {
        placingOrder.Value.OrderDate = DateTime.Now;
        daoFactory.GetOrderDao().Save(placingOrder.Value);

        facade.CurrentConversation.FinishSpan(); //finish up the current spanning conversation
        
        
        placingOrder = null; //reset the placing order to null;
        phConfirm.Visible = false;
        StartOver();
      
    }

}