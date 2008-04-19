using System;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.EntityBases
{
    [TestFixture]
    public class PersistenceTests
    {
        #region Setup/Teardown

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

        #endregion

        private BurrowFramework bf = new BurrowFramework();

        private void CommitAndStartnNewPersistentContext()
        {
            bf.CloseWorkSpace();
            bf.InitWorkSpace();
        }

        [Test, Ignore("not ready yet")]
        public void HashIdTest()
        {
            HashIdMockClass o1 = new HashIdMockClass();
            HashIdMockClass o2 = new HashIdMockClass();
            HashIdMockClass o3 = new HashIdMockClass();
            Assert.AreNotEqual(o1, o2, o1.BusinessKey + " == " + o2.BusinessKey);
            Assert.AreNotEqual(o3, o2);
            Assert.AreNotEqual(o1, o3);
            o1.Save();
            o2.Save();
            o3.Save();
            CommitAndStartnNewPersistentContext();
            HashIdMockClassDAO dao = new HashIdMockClassDAO();
            HashIdMockClass o1reloaded = dao.Get(o1.Id);
            HashIdMockClass o2reloaded = dao.Get(o2.Id);
            HashIdMockClass o3reloaded = dao.Get(o3.Id);
            Console.WriteLine(o1.GetHashCode());

            Assert.AreNotEqual(o1reloaded, o2reloaded);
            Assert.AreNotEqual(o3reloaded, o2reloaded);
            Assert.AreNotEqual(o1reloaded, o3reloaded);

            Assert.AreEqual(o1, o1reloaded, o1.GetHashCode().ToString() + " vs " + o1reloaded.GetHashCode().ToString());
            Assert.AreEqual(o2, o2reloaded);
            Assert.AreEqual(o3, o3reloaded);

            o1reloaded.Delete();
            o2reloaded.Delete();
            o3reloaded.Delete();

            CommitAndStartnNewPersistentContext();
        }
    }
}