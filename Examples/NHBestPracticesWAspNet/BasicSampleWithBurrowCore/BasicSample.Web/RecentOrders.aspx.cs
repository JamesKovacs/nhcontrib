using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BasicSample.Data;

public partial class RecentOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
            grdOrders.DataSource =
                new NHibernateDaoFactory().GetOrderDao().GetOrdersPlacedBetween(DateTime.Now.Date,
                                                                                DateTime.Now.Date.AddDays(1));
        
           grdOrders.DataBind();

    }
}
