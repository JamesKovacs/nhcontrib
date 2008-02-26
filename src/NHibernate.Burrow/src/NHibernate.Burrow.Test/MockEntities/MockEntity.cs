using System;
using System.Collections.Generic;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Test.MockEntities
{
    public class MockEntity : PersistentObjSaveDeleteSimple {
        private string name;

        public int OnPreDeletedPerformed = 0;

        public MockEntity() {
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

    public class MockDAO : GenericDAO<MockEntity> {
        public IList<MockEntity> FindByName(string name) {
            return CreateQuery("where this.Name = ?").SetString(0, name).List<MockEntity>();
        }
    }
}