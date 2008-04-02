using System;
using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Utils;

namespace BasicSample.Core.Domain
{
    public class Customer : DomainObject<string>, IHasAssignedId<string>
    {
        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Customer() {}

        public Customer(string companyName) {
            // Set the public setter instead of the private member directly so that any business 
            // rules will be carried out
            CompanyName = companyName;
        }

        /// <summary>
        /// Provides an accessor for injecting an IOrderDao so that this class does 
        /// not have to create one itself.  Can be set from a controller, using 
        /// IoC, or from another business object.  As a rule-of-thumb, I do not like
        /// domain objects to use DAOs directly; but there are exceptional cases; 
        /// therefore, this shows a way to do it without having a concrete dependency on the DAO.
        /// </summary>
        public IOrderDao OrderDao {
            get {
                if (orderDao == null) {
                    throw new MemberAccessException("OrderDao has not yet been initialized");
                }

                return orderDao;
            }
            set {
                orderDao = value;
            }
        }

        public string CompanyName {
            get { return companyName; }
            set {
                Check.Require(!string.IsNullOrEmpty(value), "A valid company name must be provided");
                companyName = value;
            }
        }

        public string ContactName {
            get { return contactName; }
            set { contactName = value; }
        }

        public IList<Order> Orders {
            get { return new List<Order>(orders).AsReadOnly(); }
            protected set { orders = value; }
        }

        public void AddOrder(Order order) {
            if (order != null && !orders.Contains(order)) {
                orders.Add(order);
            }
        }

        public void RemoveOrder(Order order) {
            if (order != null && orders.Contains(order)) {
                orders.Remove(order);
            }
        }

        /// <summary>
        /// To get all the orders ordered on a particular date, we could loop through 
        /// each item in the Orders collection.  But if a customer has thousands of 
        /// orders, we don't want all the orders to have to be loaded from the database.  
        /// Instead, we can let the data layer do the filtering for us.
        /// </summary>
        public List<Order> GetOrdersOrderedOn(DateTime orderedDate) {
            Order exampleOrder = new Order(this);
            exampleOrder.OrderDate = orderedDate;

            // Make sure you use "OrderDao" and not "orderDao" so it'll be checked for proper initialization;
            // otherwise, you may get the oh-so-fun-to-track-down "object reference" exception.
            List<Order> allMatchingOrders = OrderDao.GetByExample(exampleOrder);

            // One downside to "GetByExample" is that the NHibernate "example fetcher" is rather shallow;
            // it'll only match on primitive properties - so even though Order.OrderedBy is set, it won't
            // match on it.  So we have to go through each of the returned results looking for any that
            // were orderd by this customer.  For situations like this, it would be better to expose a 
            // more specialized IOrderDao method, but this'll work for the demonstration at hand.
            List<Order> matchingOrdersForThisCustomer = new List<Order>();

            foreach (Order matchingOrder in allMatchingOrders) {
                if (matchingOrder.OrderedBy.ID == ID) {
                    matchingOrdersForThisCustomer.Add(matchingOrder);
                }
            }

            return matchingOrdersForThisCustomer;
        }

        public void SetAssignedIdTo(string assignedId) {
            Check.Require(! string.IsNullOrEmpty(assignedId), "assignedId may not be null or empty");
            // As an alternative to Check.Require, the Validation Application Block could be used for the following
            Check.Require(assignedId.Trim().Length == 5, "assignedId must be exactly 5 characters");

            ID = assignedId.Trim().ToUpper();
        }

        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    CompanyName + "|" +
                    ContactName).GetHashCode();
        }

        private IOrderDao orderDao;
        private string companyName = "";
        private string contactName = "";
        private IList<Order> orders = new List<Order>();
    }
}
