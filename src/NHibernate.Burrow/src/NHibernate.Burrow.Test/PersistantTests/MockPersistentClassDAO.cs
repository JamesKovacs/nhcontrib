using NHibernate.Burrow.Util.DAOBases;

namespace NHibernate.Burrow.Test.PersistenceTests {
    public class MockPersistentClassDAO : GenericDAO<MockPersistentClass> {
        private static readonly MockPersistentClassDAO instance = new MockPersistentClassDAO();

        public static MockPersistentClassDAO Instance {
            get { return instance; }
        }

        // Uncomment the following method if your entity has a unique Name property 
        // public MockPersistentClass FindByName(string name) {
        //     return (MockPersistentClass) GetCriteria().Add(Expression.Eq("Name",name))
        //                             .UniqueResult();
        // }
    }
}