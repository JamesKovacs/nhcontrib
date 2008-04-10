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

        if (!IsPostBack)
        {   
            odsRecentOrders.SelectParameters["startDate"].DefaultValue = DateTime.Today.ToString();
            odsRecentOrders.SelectParameters["endDate"].DefaultValue = DateTime.Today.AddDays(1).ToString();
        }

    }
}
