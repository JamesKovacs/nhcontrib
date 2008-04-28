using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class ListHistoricalOrderSummariesPresenter : PresenterBase
    {
        public ListHistoricalOrderSummariesPresenter(IListObjectsView<HistoricalOrderSummary> view) {
            Check.Require(view != null, "view may not be null");

            this.view = view;
            this.historicalOrderSummaryDao = DaoFactory.GetHistoricalOrderSummaryDao();
        }

        public void InitViewWith(Customer customer) {
            Check.Require(customer!=null, "customerId may not be empty");

            view.ObjectsToList = historicalOrderSummaryDao.GetCustomerOrderHistoryFor(customer);
        }

        private IListObjectsView<HistoricalOrderSummary> view;
        private IHistoricalOrderSummaryDao historicalOrderSummaryDao;
    }
}
