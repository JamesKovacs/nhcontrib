using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Test.PersistantTests {
    public class HashIdMockClass : ObjWHashIdNDAOBase {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }
    }

    public class HashIdMockClassDAO : GenericDAOBase<HashIdMockClass> {
        public HashIdMockClass FindByName(string name) {
            return GetCriteria().Add(Expression.Expression.Eq("Name", name))
                .UniqueResult<HashIdMockClass>();
        }
    }
}