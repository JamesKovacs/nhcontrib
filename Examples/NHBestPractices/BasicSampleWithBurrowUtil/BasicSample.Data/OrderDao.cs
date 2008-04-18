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
     
        public IList<Order> GetOrdersPlacedBetween(DateTime startDate, DateTime endDate, int startRow, int PageSize, string sortExpression) {
            return Find(startRow, PageSize, sortExpression, PlacedBetweenCriteria(startDate,endDate) );
        }

        public int CountOrdersPlacedBetween(DateTime startDate, DateTime endDate) {
            return Count(PlacedBetweenCriteria(startDate, endDate));
        }

        private ICriterion[] PlacedBetweenCriteria(DateTime startDate, DateTime endDate) {
            return new ICriterion[]{ Expression.Le("OrderDate", endDate), 
                                    Expression.Ge("OrderDate", startDate)};
        }



    }
}
