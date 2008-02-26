using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Util.DAOBases;

namespace NHibernate.Burrow.Test.PersistenceTests {
    public class MockEntityDAO : GenericDAO<MockEntity> {
        private static readonly MockEntityDAO instance = new MockEntityDAO();

        public static MockEntityDAO Instance {
            get { return instance; }
        }

        // Uncomment the following method if your entity has a unique Name property 
        // public MockEntity FindByName(string name) {
        //     return (MockPersistentClass) GetCriteria().Add(Expression.Eq("Name",name))
        //                             .UniqueResult();
        // }
    }
}