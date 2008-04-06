using NUnit.Framework;

namespace NHibernate.Burrow.Test.Transaction {
    [TestFixture]
    public class ManualTransactionFixture {
        [Test]
        public void ManualTransactionInOneConversationTest() {
            Facade facade = new Facade();
            facade.BurrowEnvironment.ShutDown();
            foreach (IPersistenceUnitCfg cfg in facade.BurrowEnvironment.Configuration.PersistenceUnitCfgs)
                cfg.ManualTransactionManagement = false;

            facade.BurrowEnvironment.Start();
            facade.InitializeDomain();

            Assert.IsTrue(facade.GetSession().Transaction.IsActive);

            facade.CloseDomain(); 
            facade.BurrowEnvironment.ShutDown();

            foreach (IPersistenceUnitCfg cfg in facade.BurrowEnvironment.Configuration.PersistenceUnitCfgs)
                cfg.ManualTransactionManagement = true;

            facade.BurrowEnvironment.Start();

            facade.InitializeDomain();

            Assert.IsFalse(facade.GetSession().Transaction.IsActive);

            facade.CloseDomain();
        }
    }
}