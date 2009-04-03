using System;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Engine;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests
{
    [TestFixture]
    public class ConfiguratorFixture
    {
        [Test]
        public void DynamicConfigTest()
        {
            BurrowFramework bf = new BurrowFramework();
            
            TestConfigurator c = new TestConfigurator();
            bf.BurrowEnvironment.ReConfig(c);
            CheckResults();
            bf.BurrowEnvironment.ReConfig(null);

        }

        [Test, Explicit("this test can only pass when the testconfigurator is set in the configuration file - <NHibernate.Burrow customConfigurator=\"NHibernate.Burrow.Test.ConfigurationTests.TestConfigurator, NHibernate.Burrow.Test\" >")]
        public void ConfiguratorSetFromConfigFileTest()
        {
            BurrowFramework bf = new BurrowFramework();

            TestConfigurator c = (TestConfigurator) bf.BurrowEnvironment.Configuration.Configurator;
            Assert.IsNotNull(c, "configurator not set");

            CheckResults();
        }

        private void CheckResults()
        {
            BurrowFramework bf = new BurrowFramework();
			ISessionFactoryImplementor factory = (ISessionFactoryImplementor)bf.GetSessionFactory(MockPersistenceUnitCfg.MockPUName);
            Assert.IsNotNull(factory);
            Assert.AreEqual(TestConfigurator.TestAdoBatchSize, factory.Settings.AdoBatchSize);
        }
    }

    public class TestConfigurator : IConfigurator
    {
        static readonly Random  random = new Random(DateTime.Now.Millisecond);
        private static readonly int testAdoBatchSize= random.Next(10, 100);
        private static readonly int testConverstationTimeOut = random.Next(10, 100);
         
        public static int TestConverstationTimeOut
        {
            get { return testConverstationTimeOut; }
        }

        public static int TestAdoBatchSize
        {
            get { return testAdoBatchSize; }
        }

        #region IConfigurator Members

        public void Config(IBurrowConfig val)
        {
            val.PersistenceUnitCfgs.Add(new MockPersistenceUnitCfg());
        }

        public void Config(IPersistenceUnitCfg puCfg, Cfg.Configuration configuration)
        {
           if(puCfg.Name ==  MockPersistenceUnitCfg.MockPUName )
           {
                
               configuration.SetProperty("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
               configuration.SetProperty("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
               configuration.SetProperty("connection.connection_string", "Server=(local);initial catalog=NHibernateBurrow;Integrated Security=SSPI");
               configuration.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");

            configuration.SetProperty("adonet.batch_size", testAdoBatchSize.ToString());
         
           }
        }


        #endregion
    }

    //<property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
    //    <property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
    //    <property name="connection.connection_string">
    //  Server=(local);initial catalog=NHibernateBurrow;Integrated Security=SSPI
    //</property>
    //    <property name="dialect">NHibernate.Dialect.MsSql2005Dialect</property>
    //    <mapping assembly='NHibernate.Burrow.Test' /

    internal class MockPersistenceUnitCfg  : IPersistenceUnitCfg
    {
        public const string MockPUName = "Mock Persistence Unit";

        public string Name
        {
            get { return MockPUName; }
            set { throw new System.NotImplementedException(); }
        }

        public string NHConfigFile
        {
            get { return string.Empty; }
            set { throw new System.NotImplementedException(); }
        }

        public string InterceptorFactory
        {
            get { return string.Empty; }
            set { throw new System.NotImplementedException(); }
        }

        public bool AutoUpdateSchema
        {
            get { return false; }
            set { throw new System.NotImplementedException(); }
        }
    }
}