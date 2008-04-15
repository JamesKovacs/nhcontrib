using System;
using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using ProjectBase.Data;

namespace EnterpriseSample.Data
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
