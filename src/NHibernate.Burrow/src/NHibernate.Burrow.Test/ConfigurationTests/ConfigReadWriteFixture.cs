using System;
using System.Collections;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Test.MockEntities;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests {
    [TestFixture]
    public class ConfigReadWriteFixture {
        [Test]
        public void ReadNHConfigFileTest() {
            IBurrowConfig section =  Facade.Configuration;
            Assert.IsNotNull(section);
            Assert.IsTrue(section.PersistenceUnitCfgs.Count > 0);

            foreach (IPersistenceUnitCfg puSection in section.PersistenceUnitCfgs) {
                Console.WriteLine(puSection.NHConfigFile);
               Assert.IsTrue(puSection.NHConfigFile.IndexOf(".xml") > 0);
            }
        }
        [Test]
        public void WriteReadNHConfigFileTest() {
            IBurrowConfig section =  Facade.Configuration;

            Assert.IsNotNull(section);
            Assert.AreEqual(5, section.ConversationCleanupFrequency);

            Random r = new Random(3);
            int freq = r.Next(10, 100);
            section.ConversationCleanupFrequency = freq;
            Assert.AreEqual(freq, section.ConversationCleanupFrequency);
            freq = r.Next(10, 100);
            section.ConversationCleanupFrequency = freq;
            Assert.AreEqual(freq, section.ConversationCleanupFrequency);
            
        }

        [Test]
        public void ConnectionStringTest()
        {
            Facade.InitializeDomain();
            string cs = Facade.Configuration.DBConnectionString(typeof(MockEntity));
            Console.WriteLine(cs);
            Assert.IsTrue(cs.IndexOf("Server") >= 0);
            Facade.CloseDomain();
        }
    }
}