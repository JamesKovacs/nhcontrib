using System;
using System.Collections.Generic;
using BasicSample.Core.Domain;
using BasicSample.Tests.Domain;

namespace BasicSample.Tests.TestFactories
{
    public class TestOrdersFactory
    {
        public List<Order> CreateOrders() {
            List<Order> orderListing = new List<Order>();
            orderListing.Add(Order1);
            orderListing.Add(Order2);
            orderListing.Add(Order3);
            return orderListing;
        }

        private Customer OrderingCustomer {
            get {
                Customer orderingCustomer = new Customer("Acme Anvils");
                new DomainObjectIdSetter<string>().SetIdOf(orderingCustomer, "ACME");
                return orderingCustomer;
            }
        }

        private Customer SomeOtherCustomer {
            get {
                Customer someOtherCustomer = new Customer("Chow Mein, Inc.");
                new DomainObjectIdSetter<string>().SetIdOf(someOtherCustomer, "MEIN");
                return someOtherCustomer;
            }
        }

        private Order Order1 {
            get {
                Order order = new Order(OrderingCustomer);
                order.ShipToName = "Road Runner";
                order.OrderDate = new DateTime(2005, 1, 11);
                return order;
            }
        }

        private Order Order2 {
            get {
                Order order = new Order(SomeOtherCustomer);
                order.ShipToName = "Hollywood";
                order.OrderDate = new DateTime(2005, 1, 11);
                return order;
            }
        }

        private Order Order3 {
            get {
                Order order = new Order(OrderingCustomer);
                order.ShipToName = "Fredericks";
                order.OrderDate = new DateTime(2005, 1, 11);
                return order;
            }
        }
    }
}
