using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using ProjectBase.Data;

namespace EnterpriseSample.Core.DataInterfaces
{
    /// <summary>
    /// This is not a typical DAO in the fact that it does not implement <see cref="IDao{T,IdT}" />
    /// but it can still be a participant of the <see cref="IDaoFactory" />.
    /// </summary>
    public interface IHistoricalOrderSummaryDao
    {
        List<HistoricalOrderSummary> GetCustomerOrderHistoryFor(Customer customer);
    }
}
