using NUnit.Framework;

namespace NHibernate.Burrow.Test.Transaction
{
    [TestFixture]
    public class ManualTransactionFixture
    {
        [Test]
        public void ManualTransactionInOneConversationTest()
        {
            BurrowFramework bf = new BurrowFramework();
            bf.BurrowEnvironment.ShutDown();
            foreach (IPersistenceUnitCfg cfg in bf.BurrowEnvironment.Configuration.PersistenceUnitCfgs)
            {
                cfg.ManualTransactionManagement = false;
            }

            bf.BurrowEnvironment.Start();
            bf.InitWorkSpace();

            Assert.IsTrue(bf.GetSession().Transaction.IsActive);

            bf.CloseWorkSpace();
            bf.BurrowEnvironment.ShutDown();

            foreach (IPersistenceUnitCfg cfg in bf.BurrowEnvironment.Configuration.PersistenceUnitCfgs)
            {
                cfg.ManualTransactionManagement = true;
            }

            bf.BurrowEnvironment.Start();

            bf.InitWorkSpace();

            Assert.IsFalse(bf.GetSession().Transaction.IsActive);

            bf.CloseWorkSpace();
        }
    }
}