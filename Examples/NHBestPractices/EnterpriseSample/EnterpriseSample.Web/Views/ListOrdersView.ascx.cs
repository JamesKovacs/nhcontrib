using System.Collections.Generic;
using System.Web.UI;
using EnterpriseSample.Core.Domain;

public partial class Views_ListOrdersView : UserControl
{
    public IList<Order> Orders {
        set {
            grdOrders.DataSource = value;
            grdOrders.DataBind();
        }
    }
}
