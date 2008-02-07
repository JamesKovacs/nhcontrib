using System;
using System.Collections.Generic;
using NHibernate.Burrow.NHDomain;

namespace NHibernate.Burrow.Test.PersistantTests {
    public class MockPersistantClass : PersistantObjSaveDeleteSimple {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int OnPreDeletedPerformed = 0;

        protected override void OnPreDeleted(object sender, System.EventArgs e)
        {
            OnPreDeletedPerformed++;
            base.OnPreDeleted(sender, e);
            
        }
        
        public MockPersistantClass() {
            Name = string.Empty;
        }
    }

    public class MockDAO : GenericDAOBase<MockPersistantClass> {
        public IList<MockPersistantClass> FindByName(string name) {
            return CreateQuery("where this.Name = ?").SetString(0, name).List<MockPersistantClass>();
        }
    }
    
    
}