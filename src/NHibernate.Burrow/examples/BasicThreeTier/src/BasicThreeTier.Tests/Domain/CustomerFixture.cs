using System;
using BasicThreeTier.Core.Dao;
using BasicThreeTier.Core.Domain;
using NUnit.Framework;

namespace BasicThreeTier.Tests.Domain
{
    [TestFixture]
    public class CustomerFixture : BootStrapTestCaseBase
    {
        [Test]
        public void CRUDTest()
        {
            string contactName = "someContact" + DateTime.Now.Second;
            Customer c = new Customer(contactName);
            CustomerDAO cdao = new CustomerDAO();
            cdao.Save(c);
            
            RestartBurrowEnvironment();

            c = cdao.Get(c.Id);
            Assert.AreEqual(contactName, c.ContactName);

            cdao.Delete(c);
        }
    }
}