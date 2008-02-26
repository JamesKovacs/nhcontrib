using System;
using System.Collections;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Test.MockEntities;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests {
    [TestFixture]
    public class ConfigReadingTest {
        [Test]
        public void ReadNHConfigFileTest() {
            NHibernateBurrowCfgSection section = NHibernateBurrowCfgSection.GetInstance();
            Assert.IsNotNull(section);
            Assert.IsTrue(section.PersistenceUnits.Count > 0);

            foreach (PersistenceUnitElement puSection in section.PersistenceUnits) {
                Console.WriteLine(puSection.NHConfigFile);
               Assert.IsTrue(puSection.NHConfigFile.IndexOf(".xml") > 0);
            }
        }

        [Test]
        public void ConnectionStringTest()
        {
            Facade.InitializeDomain();
            string cs = Config.DBConnectionString(typeof(MockEntity));
            Console.WriteLine(cs);
            Assert.IsTrue(cs.IndexOf("Server") >= 0);
            Facade.CloseDomain();
        }
    }
}