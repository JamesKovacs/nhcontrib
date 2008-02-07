using System;
using System.Collections.Generic;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Test.PersistantTests {
    public class MockPersistantClass : PersistantObjSaveDeleteSimple {
        private string name;

        public int OnPreDeletedPerformed = 0;

        public MockPersistantClass() {
            Name = string.Empty;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        protected override void OnPreDeleted(object sender, EventArgs e) {
            OnPreDeletedPerformed++;
            base.OnPreDeleted(sender, e);
        }
    }

    public class MockDAO : GenericDAOBase<MockPersistantClass> {
        public IList<MockPersistantClass> FindByName(string name) {
            return CreateQuery("where this.Name = ?").SetString(0, name).List<MockPersistantClass>();
        }
    }
}