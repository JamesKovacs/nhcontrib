using System.Collections.Generic;
using BasicSample.Core.Domain;

namespace BasicSample.Core.DataInterfaces
{
    /// <summary>
    /// This is not a typical DAO in the fact that it does not implement <see cref="IDao{TypeOfListItem, IdT}" />
    /// but it can still be a participant of the <see cref="IDaoFactory" />.
    /// </summary>
    public interface IHistoricalOrderSummaryDao
    {
        List<HistoricalOrderSummary> GetCustomerOrderHistoryFor(string customerId);
    }
}
