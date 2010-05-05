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
        private IRepository<Person> personRepository;
        private IRepository<Address> addressRepository;

        [TestFixtureSetUp]
        public void SetUp()
        {
            applicationContext = new XmlApplicationContext("spring-config.xml");
            personRepository = (IRepository<Person>)applicationContext.GetObject("PersonRepository");
            addressRepository = (IRepository<Address>)applicationContext.GetObject("AddressRepository");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ((XmlApplicationContext)applicationContext).Dispose();
        }

        //[Test]
        public void testDbProvider()
        {
            IDbProvider dbProvider = (IDbProvider)applicationContext.GetObject("DbProvider");
            using(IDbConnection connection = dbProvider.CreateConnection())
            {
                Assert.NotNull(connection);
            }
        }


        //[Test]
        public void TestSessionFactoryStartedOk()
        {
            ISessionFactory sessionFactory = (ISessionFactory)applicationContext.GetObject("NHibernateSessionFactory");
            Assert.IsNotNull(sessionFactory);
        }


        [Test]
        public void TestSessionFactory()
        {
            //AuditEventListener lis = (AuditEventListener)applicationContext.GetObject("enversEventListener");

            Address address = new Address{number="22", street="Valea Calugareasca"};
            addressRepository.Add(address);
            Assert.IsTrue(address.id != 0);
            long id1 = address.id;
            
            Person pers = new Person{firstName = "Ion", lastName = "Gheorghe", address = address};
            personRepository.Add(pers);
            Assert.IsTrue(pers.id != 0);
            
            address.number = "23";
            addressRepository.Update(address);

            address.number = "24";
            addressRepository.Update(address);

            address = new Address { number = "45", street = "Strada Strada" };

            addressRepository.Add(address);

            address = addressRepository.GetById(id1);
            Assert.IsTrue(address.number == "23" && address.street == "Valea Calugareasca");


        }
    }
}
