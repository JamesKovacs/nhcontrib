using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using EnterpriseSample.Web;

public partial class Views_ListHistoricalOrderSummariesView : BaseUserControl, IListObjectsView<HistoricalOrderSummary>
{
    public IList<HistoricalOrderSummary> ObjectsToList {
        set {
            grdProductsOrdered.DataSource = value;
            grdProductsOrdered.DataBind();
        }
    }
}
