using System;
using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using NHibernate.Criterion;
using Order=BasicSample.Core.Domain.Order;

namespace BasicSample.Data
{
    /// <summary>
    /// Concrete DAO for accessing instances of <see cref="Core.Domain.Order" /> from DB.
    /// </summary>
    public class OrderDao : AbstractNHibernateDao<Order, long>, IOrderDao
    {
        public List<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate) {
            return GetByCriteria(Expression.Le("OrderDate", endDate), 
                                Expression.Ge("OrderDate", startDate));
        }
    }
}
