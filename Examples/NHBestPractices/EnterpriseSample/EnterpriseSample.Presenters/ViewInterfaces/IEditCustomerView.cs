using System.Collections.Generic;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Presenters.ViewInterfaces
{
    public interface IEditCustomerView
    {
        Customer Customer { set; }
        void UpdateValuesOn(Customer customer);

        IList<Order> Orders { set;}
        IList<HistoricalOrderSummary> HistoricalOrders { set;}
    }
}
