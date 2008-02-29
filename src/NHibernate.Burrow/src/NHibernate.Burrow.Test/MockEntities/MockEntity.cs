using System;
using System.Collections.Generic;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;
using NHibernate.Expressions;

namespace NHibernate.Burrow.Test.MockEntities
{
    public class MockEntity : PersistentObjSaveDeleteSimple
    {
        private string name;
        private int number;

        private int preDeletedPerformed = 0;

        public MockEntity()
        {
            Name = string.Empty;
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int PreDeletedPerformed
        {
            get { return preDeletedPerformed; }
            set { preDeletedPerformed = value; }
        }

        protected override void OnPreDeleted(object sender, EventArgs e)
        {
            preDeletedPerformed++;
            base.OnPreDeleted(sender, e);
        }
    }

    public class MockDAO : GenericDAO<MockEntity>
    {
        public IList<MockEntity> FindByName(string name)
        {
            return FindByCriterion(Expression.Eq("Name", name));
        }
    }
}