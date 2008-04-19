using System;
using System.Collections.Generic;
using BasicSample.Core.DataInterfaces;
using BasicSample.Core.Domain;
using BasicSample.Data;
using NUnit.Framework;

namespace BasicSample.Tests.Data
{
    [TestFixture]
    [Category("Database Tests")]
    public class CustomerDaoTests : NHibernateTestCase
    {
        [Test]
        public void TestGetById() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();

            Customer foundCustomer = customerDao.GetById(TestGlobals.TestCustomer.ID, false);
            Assert.AreEqual(TestGlobals.TestCustomer.CompanyName, foundCustomer.CompanyName);
        }

        [Test]
        public void TestGetOrdersShippedTo() {
            IDaoFactory daoFactory = new NHibernateDaoFactory();
            ICustomerDao customerDao = daoFactory.GetCustomerDao();

            Customer customer = customerDao.GetById(TestGlobals.TestCustomer.ID, false);
            // Give the customer its DAO dependency via a public setter
            customer.OrderDao = daoFactory.GetOrderDao();
            IList<Order> ordersMatchingDate = customer.GetOrdersOrderedOn(new DateTime(1998, 3, 10));

            Assert.AreEqual(1, ordersMatchingDate.Count);
            Assert.AreEqual(10937, ordersMatchingDate[0].ID);
        }
    }
}
