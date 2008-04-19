using System.Collections.Generic;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Test.MockEntities
{
    public class MockEntityDAO
    {
        private static readonly MockEntityDAO instance = new MockEntityDAO();
        private System.Type persistentClass = typeof (MockEntity);

        public static MockEntityDAO Instance
        {
            get { return instance; }
        }

        private ISession Sess
        {
            get { return new BurrowFramework().GetSession(persistentClass); }
        }

        public IList<MockEntity> FindByName(string name)
        {
            return Crit().Add(Expression.Eq("Name", name)).List<MockEntity>();
        }

        protected ICriteria Crit()
        {
            return Sess.CreateCriteria(persistentClass);
        }

        public void Save(MockEntity me)
        {
            Sess.Save(me);
        }

        public void Delete(MockEntity me)
        {
            Sess.Delete(me);
        }

        public MockEntity Get(object id)
        {
            return Sess.Get<MockEntity>(id);
        }

        public IList<MockEntity> FindAll()
        {
            return Crit().List<MockEntity>();
        }
    }
}