using System.Collections.Generic;
using System.Web.UI;
using EnterpriseSample.Core.Domain;

public partial class Views_ListHistoricalOrderSummariesView : UserControl
{
    public IList<HistoricalOrderSummary> HistoricalOrderSummary
    {
        set {
            grdProductsOrdered.DataSource = value;
            grdProductsOrdered.DataBind();
        }
    }
}
