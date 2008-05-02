using System;
using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using NUnit.Framework;
using ProjectBase.Utils;

namespace EnterpriseSample.Tests.Domain
{
    [TestFixture]
    public class CustomerTests : TestCase
    {
        [Test]
        public void CanCreateCustomer() {
            Customer customer = new Customer("Acme Anvil");
            Assert.AreEqual("Acme Anvil", customer.CompanyName);

            customer.CompanyName = "Acme 2";
            Assert.AreEqual("Acme 2", customer.CompanyName);

            Assert.AreEqual("", customer.ContactName);
            customer.ContactName = "Billy";
            Assert.AreEqual("Billy", customer.ContactName);
        }

        [Test]
        [ExpectedException(typeof(PreconditionException))]
        public void CannotCreateCustomerWithoutCompanyName() {
            new Customer("");
        }

        [Test]
        public void CanAddRemoveOrders() {
            Customer customer = new Customer("Acme Anvil");
            Assert.AreEqual(0, customer.Orders.Count);

            Order order = new Order(customer);
            Assert.AreEqual(1, customer.Orders.Count);

            // Was already added via the order's constructor
            customer.AddOrder(order);
            Assert.AreEqual(1, customer.Orders.Count);

            Assert.IsTrue(customer.Orders.Contains(order));
            customer.RemoveOrder(order);
            Assert.IsFalse(customer.Orders.Contains(order));
        }

        //[Test]
        //public void CanGetOrdersOrderedOnDateUsingStubbedDao() {
        //    Customer customer = new Customer("Acme Anvils");
        //    new DomainObjectIdSetter<string>().SetIdOf(customer, "ACME");
        //    customer.OrderDao = new OrderDaoStub();

        //    List<Order> matchingOrders = customer.GetOrdersOrderedOn(new DateTime(2005, 1, 11));
        //    Assert.AreEqual(2, matchingOrders.Count);
        //}

        //[Test]
        //public void CanGetOrdersOrderedOnDateUsingMockedDao() {
        //    Customer customer = new Customer("Acme Anvils");
        //    new DomainObjectIdSetter<string>().SetIdOf(customer, "ACME");
        //    customer.OrderDao = DaoFactory.GetOrderDao();

        //    IOrderDao orderDao = new MockOrderDaoFactory().CreateMockOrderDao();
        //    Assert.IsNotNull(orderDao);

        //    List<Order> matchingOrders = customer.GetOrdersOrderedOn(new DateTime(2005, 1, 11));
        //    Assert.AreEqual(2, matchingOrders.Count);
        //}

        [Test]
        public void CanCompareCustomers() {
            Customer customerA = new Customer("Acme");
            Customer customerB = new Customer("Anvil");
            
            Assert.AreNotEqual(customerA, null);
            Assert.AreNotEqual(customerA, customerB);

            customerA.SetAssignedIdTo("AAAAA");
            customerB.SetAssignedIdTo("AAAAA");

            // Even though the "business value signatures" are different, the persistent IDs 
            // were the same.  Call me crazy, but I put that much trust into IDs.
            Assert.AreEqual(customerA, customerB);

            Customer customerC = new Customer("Acme");

            // Since customerA has an ID but customerC doesn't, their signatures will be compared
            Assert.AreEqual(customerA, customerC);

            customerC.ContactName = "Coyote";

            // Signatures are now different
            Assert.AreNotEqual(customerA, customerC);

            // customerA.Equals(customerB) because they have the same ID.
            // customerA.Equals(customerC) because they have the same signature.
            // ! customerB.Equals(customerC) because we can't compare their IDs, 
            // since customerC is transient, and their signatures are different.
            Assert.AreNotEqual(customerB, customerC);
        }
    }
}
