using System;
using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Data;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Data
{
    [TestFixture, Ignore("need refactor")]
    [Category("Database Tests")]
    public class CustomerDaoTests : NHibernateTestCase
    {
        [Test]
        public void CanGetById() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();

            Customer foundCustomer = customerDao.GetById(TestGlobals.TestCustomer.ID, false);
            Assert.AreEqual(TestGlobals.TestCustomer.CompanyName, foundCustomer.CompanyName);
        }

        [Test]
        public void CanGetByExample() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();
            Customer exampleCustomer = new Customer(TestGlobals.TestCustomer.CompanyName);
            Customer foundCustomer = customerDao.GetUniqueByExample(exampleCustomer);
            Assert.AreEqual(TestGlobals.TestCustomer.CompanyName, foundCustomer.CompanyName);
        }

        [Test, Explicit]
        public void CanGetOrdersShippedTo() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();
            IOrderDao orderDao = daoFactory.GetOrderDao();

            Customer customer = customerDao.GetById(TestGlobals.TestCustomer.ID, false);
            // Give the customer its DAO dependency via a public setter
            IList<Order> ordersMatchingDate = orderDao.GetOrdersOrderedOn(customer, new DateTime(1998, 3, 10));

            Assert.AreEqual(1, ordersMatchingDate.Count);
            Assert.AreEqual(10937, ordersMatchingDate[0].ID);
        }
    }
}
