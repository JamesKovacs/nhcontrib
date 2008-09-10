using System.Collections.Generic;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Test.MultiDB
{
    public class MockEntity2DAO
    {
        private static readonly MockEntity2DAO instance = new MockEntity2DAO();
        private System.Type persistentClass = typeof (MockEntity2);

        public static MockEntity2DAO Instance
        {
            get { return instance; }
        }

        private ISession Sess
        {
            get { return new BurrowFramework().GetSession(persistentClass); }
        }

        public IList<MockEntity2> FindByName(string name)
        {
            return Crit().Add(Expression.Eq("Name", name)).List<MockEntity2>();
        }

        protected ICriteria Crit()
        {
            return Sess.CreateCriteria(persistentClass);
        }

        public void Save(MockEntity2 me)
        {
            Sess.Save(me);
        }

        public void Delete(MockEntity2 me)
        {
            Sess.Delete(me);
        }

        public MockEntity2 Get(object id)
        {
            return Sess.Get<MockEntity2>(id);
        }

        public IList<MockEntity2> FindAll()
        {
            return Crit().List<MockEntity2>();
        }
    }
}