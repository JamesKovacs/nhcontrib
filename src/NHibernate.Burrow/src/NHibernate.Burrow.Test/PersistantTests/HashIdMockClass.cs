using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;
using NHibernate.Expressions;

namespace NHibernate.Burrow.Test.PersistenceTests {
    public class HashIdMockClass : ObjWHashIdNDAOBase {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }
    }

    public class HashIdMockClassDAO : GenericDAO<HashIdMockClass> {
        public HashIdMockClass FindByName(string name) {
            return GetCriteria().Add(Expression.Eq("Name", name))
                .UniqueResult<HashIdMockClass>();
        }
    }
}