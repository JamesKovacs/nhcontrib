using System.Collections.Generic;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Test.MockEntities
{
    public class MockEntityDAO : GenericDAO<MockEntity>
    {
        private static readonly MockEntityDAO instance = new MockEntityDAO();

        public static MockEntityDAO Instance
        {
            get { return instance; }
        }

        public IList<MockEntity> FindByName(string name)
        {
            return Find(Expression.Eq("Name", name));
        }
    }
}