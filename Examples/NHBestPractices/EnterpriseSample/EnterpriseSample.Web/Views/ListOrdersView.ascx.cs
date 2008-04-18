using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Web;

public partial class Views_ListOrdersView : BaseUserControl, IListObjectsView<Order>
{
    public IList<Order> ObjectsToList {
        set {
            grdOrders.DataSource = value;
            grdOrders.DataBind();
        }
    }
}
