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

        /// <summary>
        /// To get all the orders ordered on a particular date, we could loop through 
        /// each item in the Orders collection.  But if a customer has thousands of 
        /// orders, we don't want all the orders to have to be loaded from the database.  
        /// Instead, we can let the data layer do the filtering for us.
        /// </summary>
        public IList<Order> GetOrdersOrderedOn(Customer customer, DateTime orderedDate)
        {
            Order exampleOrder = new Order(customer);
            exampleOrder.OrderDate = orderedDate;

            // Make sure you use "OrderDao" and not "orderDao" so it'll be checked for proper initialization;
            // otherwise, you may get the oh-so-fun-to-track-down "object reference" exception.
            List<Order> allMatchingOrders = this.GetByExample(exampleOrder);

            // One downside to "GetByExample" is that the NHibernate "example fetcher" is rather shallow;
            // it'll only match on primitive properties - so even though Order.OrderedBy is set, it won't
            // match on it.  So we have to go through each of the returned results looking for any that
            // were orderd by this customer.  For situations like this, it would be better to expose a 
            // more specialized IOrderDao method, but this'll work for the demonstration at hand.
            List<Order> matchingOrdersForThisCustomer = new List<Order>();

            foreach (Order matchingOrder in allMatchingOrders)
            {
                if (matchingOrder.OrderedBy.ID == customer.ID)
                {
                    matchingOrdersForThisCustomer.Add(matchingOrder);
                }
            }

            return matchingOrdersForThisCustomer;
        }
    }
}
