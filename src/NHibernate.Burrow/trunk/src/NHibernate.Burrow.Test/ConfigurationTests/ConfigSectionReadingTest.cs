using System;
using System.Collections;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests {
    [TestFixture]
    public class ConfigSectionReadingTest  {
        [Test]
        public void Test() {
            MHDomainTemplateSection section = MHDomainTemplateSection.GetInstance();
            Assert.IsNotNull(section);
            Assert.IsTrue(section.PersistantUnits.Count > 0 );

            foreach (PUSection puSection in section.PersistantUnits)
            {
               Assert.IsTrue(  puSection.DomainLayerAssemblies.Count > 0 );
                foreach (DomainLayerAssemblyElement dle in puSection.DomainLayerAssemblies) {
                    Console.WriteLine(dle.Name + " - " + dle.DomianSessionFactoryName);
                }

                foreach (DictionaryEntry entry in puSection.ORMFrameworkSettingsDict) {
                    Console.WriteLine(entry.Key + " - "  + entry.Value);
                }
            }
        }
    }
}