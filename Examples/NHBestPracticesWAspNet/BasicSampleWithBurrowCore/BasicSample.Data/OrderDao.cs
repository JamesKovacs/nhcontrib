using System;
using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;

namespace BasicSample.Data
{
    /// <summary>
    /// Concrete DAO for accessing instances of <see cref="Order" /> from DB.
    /// </summary>
    public class OrderDao : AbstractNHibernateDao<Order, long>, IOrderDao
    {
        public List<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate) {
            throw new NotImplementedException();
        }
    }
}
