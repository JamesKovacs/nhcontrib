using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Spring.Context.Support;
using Spring.Context;
using NHibernate;
using Envers.Net.Model;
using Envers.Net.Repository;
using Spring.Data.Common;
using System.Data;
using NHibernate.Envers.Event;

namespace APDRP_NHibernatePOC_Test
{
    [TestFixture]
    public class HibernateStartedTest
    {
        private IApplicationContext applicationContext;

        [TestFixtureSetUp]
        public void SetUp()
        {
            applicationContext = new XmlApplicationContext("spring-config.xml");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ((XmlApplicationContext)applicationContext).Dispose();
        }

        [Test]
        public void testDbProvider()
        {
            IDbProvider dbProvider = (IDbProvider)applicationContext.GetObject("DbProvider");
            using(IDbConnection connection = dbProvider.CreateConnection())
            {
                Assert.NotNull(connection);
            }
        }


        [Test]
        public void TestSessionFactoryStartedOk()
        {
            ISessionFactory sessionFactory = (ISessionFactory)applicationContext.GetObject("NHibernateSessionFactory");
            Assert.IsNotNull(sessionFactory);
        }


        [Test]
        public void TestSessionFactory()
        {
            IApplicationContext applicationContext = new XmlApplicationContext("spring-config.xml");
            //AuditEventListener lis = (AuditEventListener)applicationContext.GetObject("enversEventListener");
            IRepository<Person> personRepository = (IRepository<Person>)applicationContext.GetObject("PersonRepository");
            IRepository<Address> addressRepository = (IRepository<Address>)applicationContext.GetObject("AddressRepository");

            Address address = new Address{number="22", street="Valea Calugareasca"};
            addressRepository.Add(address);
            Assert.IsTrue(address.id != 0);
        }
    }
}
