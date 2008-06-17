using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ManualTransaction
{
    [TestFixture]
    public class ManualTransactionFexture
    {
        #region Setup/Teardown

        [TearDown]
        public void TearDown()
        {
            new SchemaUtil().DropSchemas(false, true);
            bf.CloseWorkSpace();
        }

        [SetUp]
        public void Setup()
        {
            bf.InitStickyWorkSpace();
            new SchemaUtil().CreateSchemas(false, true);
        }

        #endregion

        private BurrowFramework bf = new BurrowFramework();

        [Test]
        public void MultipleTransactionTest()
        {
            ITransactionManager tm = bf.CurrentConversation.TransactionManager;
            tm.Begin();
            MockEntity mo1 = new MockEntity();
            mo1.Save();
            tm.Commit();

            tm.Begin();
            MockEntity mo2 = new MockEntity();
            mo2.Save();
            tm.Commit();

            tm.Begin();
            Assert.IsNotNull(MockEntityDAO.Instance.Get(mo1.Id));
            Assert.IsNotNull(MockEntityDAO.Instance.Get(mo2.Id));

            tm.Commit();

            TearDown();
        }

        [Test]
        public void RollBackShouldCloseWorkSpaceTest()
        {
            ITransactionManager tm = bf.CurrentConversation.TransactionManager;

            tm.Begin();
            MockEntity mo3 = new MockEntity();
            mo3.Save();
            tm.Rollback();
            Assert.IsNull(bf.CurrentConversation);
            bf.InitWorkSpace();
            Assert.IsNull(MockEntityDAO.Instance.Get(mo3.Id), "Rolled back transaction did get commit");
        }
    }
}