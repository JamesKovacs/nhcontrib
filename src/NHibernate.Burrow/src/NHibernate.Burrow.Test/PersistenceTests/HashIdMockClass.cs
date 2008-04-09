using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Test.PersistenceTests {
    public class HashIdMockClass : EntityWHashIdBase {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        protected override void PreDelete() {
            // do nothing
        }

        public void Save() {
            new HashIdMockClassDAO().Save(this);
        }
    }

    public class HashIdMockClassDAO : GenericDAO<HashIdMockClass> {
        public HashIdMockClass FindByName(string name) {
            return CreateCriteria().Add(Expression.Eq("Name", name))
                .UniqueResult<HashIdMockClass>();
        }
    }
}