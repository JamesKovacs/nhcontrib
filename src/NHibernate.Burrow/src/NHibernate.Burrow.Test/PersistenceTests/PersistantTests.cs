using System;
using System.Collections.Generic;
using NHibernate.Burrow;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.TestUtil;
using NHibernate.Burrow.Util.DAOBases;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.PersistenceTests {
    [TestFixture]
    public class PersistenceTests : TestBase {
        [Test]
        public void CRUDTest() {
            string mockName = new Random(1000).Next().ToString();
            MockEntity m = new MockEntity();
            m.Name = mockName;
            m.Save();
            GenericDAO<MockEntity> d = new GenericDAO<MockEntity>();
            Assert.AreEqual(d.FindById(m.Id), m);
            Assert.AreEqual(mockName, d.FindById(m.Id).Name);
            m.Delete();
            SessionManager.GetInstance().GetSession().Flush();
            Assert.IsNull(d.FindById(m.Id));
        }

        [Test]
        public void DAOTest() {
            MockEntity m = new MockEntity();
            string name = RandomStringGenerator.GenerateLetterStrings(5);
            m.Name = name;
            m.Save();
            IList<MockEntity> result = new MockDAO().FindByName(name);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(m, result[0]);
            m.Delete();
        }

        [Test]
        public void HashIdTest() {
            HashIdMockClass o1 = new HashIdMockClass();
            HashIdMockClass o2 = new HashIdMockClass();
            HashIdMockClass o3 = new HashIdMockClass();
            Assert.AreNotEqual(o1, o2, o1.BusinessKey + " == " + o2.BusinessKey);
            Assert.AreNotEqual(o3, o2);
            Assert.AreNotEqual(o1, o3);
            o1.DAO.Save();
            o2.DAO.Save();
            o3.DAO.Save();
            CommitAndClearSession();
            HashIdMockClassDAO dao = new HashIdMockClassDAO();
            HashIdMockClass o1reloaded = dao.FindById(o1.Id);
            HashIdMockClass o2reloaded = dao.FindById(o2.Id);
            HashIdMockClass o3reloaded = dao.FindById(o3.Id);
            Console.WriteLine(o1.GetHashCode());

            Assert.AreNotEqual(o1reloaded, o2reloaded);
            Assert.AreNotEqual(o3reloaded, o2reloaded);
            Assert.AreNotEqual(o1reloaded, o3reloaded);

            Assert.AreEqual(o1, o1reloaded, o1.GetHashCode().ToString() + " vs " + o1reloaded.GetHashCode().ToString());
            Assert.AreEqual(o2, o2reloaded);
            Assert.AreEqual(o3, o3reloaded);

            o1reloaded.DAO.Delete();
            o2reloaded.DAO.Delete();
            o3reloaded.DAO.Delete();

            CommitAndClearSession();
        }

        [Test]
        public void PreDeleteTest() {
            MockEntity m = new MockEntity();
            m.Save();
            GenericDAO<MockEntity> d = new GenericDAO<MockEntity>();
            Assert.AreEqual(d.FindById(m.Id), m);
            m.Delete();
            Assert.AreEqual(1, m.OnPreDeletedPerformed);
        }
    }
}