using System;
using System.Collections;
using System.Configuration;
using NHibernate.Burrow.Configuration;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests {
    [TestFixture]
    public class ConfigSectionReadingTest {
        [Test]
        public void Test() {
            MHDomainTemplateSection section = MHDomainTemplateSection.GetInstance();
            Assert.IsNotNull(section);
            Assert.IsTrue(section.PersistantUnits.Count > 0);

            foreach (PUSection puSection in section.PersistantUnits) {
                Assert.IsTrue(puSection.DomainAssemblies.Count > 0);
                foreach (DomainAssemblySection dle in puSection.DomainAssemblies)
                    foreach (KeyValueConfigurationElement setting in dle.DomainAssemblySettings)
                        Console.WriteLine(setting.Key + " - " + setting.Value);

                foreach (DictionaryEntry entry in puSection.ORMFrameworkSettingsDict)
                    Console.WriteLine(entry.Key + " - " + entry.Value);
            }
        }
    }
}