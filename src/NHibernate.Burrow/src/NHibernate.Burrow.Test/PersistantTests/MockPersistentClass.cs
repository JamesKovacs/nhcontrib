using System;
using System.Collections.Generic;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Test.PersistenceTests {
    public class MockPersistentClass : PersistentObjSaveDeleteSimple {
        private string name;

        public int OnPreDeletedPerformed = 0;

        public MockPersistentClass() {
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

    public class MockDAO : GenericDAO<MockPersistentClass> {
        public IList<MockPersistentClass> FindByName(string name) {
            return CreateQuery("where this.Name = ?").SetString(0, name).List<MockPersistentClass>();
        }
    }
}