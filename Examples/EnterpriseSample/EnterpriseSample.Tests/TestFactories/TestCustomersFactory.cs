using System;
using System.Collections.Generic;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Tests.TestFactories
{
    public class TestCustomersFactory
    {
        public List<Customer> CreateCustomers() {
            List<Customer> customerListing = new List<Customer>();
            customerListing.Add(Customer1);
            customerListing.Add(Customer2);
            customerListing.Add(Customer3);
            return customerListing;
        }

        public Customer CreateCustomer() {
            return Customer1;
        }

        private Customer Customer1 {
            get {
                Customer customer = TestGlobals.TestCustomer;

                List<Order> customerOrders = new TestOrdersFactory().CreateOrders();

                // Cool little example of using a delegate to process a list
                customerOrders.ForEach(delegate(Order order) { customer.AddOrder(order); });

                return customer;
            }
        }

        private Customer Customer2 {
            get {
                Customer customer = new Customer("B");
                return customer;
            }
        }

        private Customer Customer3 {
            get {
                Customer customer = new Customer("C");
                return customer;
            }
        }
    }
}
