using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.MultiDB
{
    [TestFixture]
    public class MultiDBFixture
    {
        private BurrowFramework bf;
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            bf = new BurrowFramework();
            bf.BurrowEnvironment.ShutDown();
            PersistenceUnitElement item = new PersistenceUnitElement();
            item.NHConfigFile = "~/MultiDB/SecondPU.Cfg.xml";
            PersistenceUnitElement item2 = new PersistenceUnitElement();
            item2.NHConfigFile = "~/MultiDB/FirstPU.Cfg.xml";
            
            bf.BurrowEnvironment.Configuration.PersistenceUnitCfgs.Clear();
            bf.BurrowEnvironment.Configuration.PersistenceUnitCfgs.Add(item);
            bf.BurrowEnvironment.Configuration.PersistenceUnitCfgs.Add(item2);
            bf.BurrowEnvironment.Start();
           new  SchemaUtil().CreateSchemas(false, true);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            new SchemaUtil().DropSchemas(false,true);
        }

        [SetUp]
        public void Setup()
        {
            bf.InitWorkSpace();
        }
        
        [TearDown]
        public void TearDown()
        {
            bf.CloseWorkSpace();
        }

        protected void RestartBF()
        {
            bf.CloseWorkSpace();
            bf.InitWorkSpace();

        }

        [Test]
        public void Crud()
        {
            MockEntity2 m2 = new MockEntity2();
            m2.Save();
           RestartBF();
           MockEntity m = new MockEntity();
           m.Save();
           RestartBF();
           Assert.IsNotNull(  MockEntity2DAO.Instance.Get(m2.Id));
           Assert.IsNotNull(  MockEntityDAO.Instance.Get(m.Id));

        }
    }
}