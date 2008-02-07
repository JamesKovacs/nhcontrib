using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.PersistantTests
{
    [TestFixture]
    public class PersistantTests : NHTestBase
    {
      
        
        [Test]
        public void CRUDTest() {
            string mockName = new Random(1000).Next().ToString();
            MockPersistantClass m = new MockPersistantClass();
            m.Name = mockName;
            m.Save();
            GenericDAO<MockPersistantClass> d = new GenericDAO<MockPersistantClass>();
            Assert.AreEqual(d.FindById(m.Id), m);
            Assert.AreEqual(mockName, d.FindById(m.Id).Name);
            m.Delete();
            SessionManager.Instance.GetSession().Flush();
            Assert.IsNull(d.FindById(m.Id));
        }
        
        [Test]
        public void DAOTest() {
            MockPersistantClass m = new MockPersistantClass();
            string name =  RandomStringGenerator.GenerateLetterStrings(5);
            m.Name = name;
            m.Save();
            IList<MockPersistantClass> result = new MockDAO().FindByName(name);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(m, result[0]);
            m.Delete();
        }
        
        [Test]
        public void PreDeleteTest() {
            MockPersistantClass m = new MockPersistantClass();
            m.Save();
            GenericDAO<MockPersistantClass> d = new GenericDAO<MockPersistantClass>();
            Assert.AreEqual(d.FindById(m.Id), m);
            m.Delete();
            Assert.AreEqual(1, m.OnPreDeletedPerformed);
        }
        
        [Test]
        public void HashIdTest(){
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
        
    }
}
