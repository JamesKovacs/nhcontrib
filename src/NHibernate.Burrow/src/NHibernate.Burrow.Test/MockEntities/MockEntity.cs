using System;
using System.Collections.Generic;
using NHibernate.Burrow.Test.UtilTests.DAO;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Burrow.Util.EntityBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Test.MockEntities
{
    public class MockEntity :  IDeletable
    {
        private int id;

        public int Id {
            get { return id; }
            set { id = value; }
        }

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

        public bool Delete() {
              preDeletedPerformed++;
               new MockEntityDAO().Delete(this);
            return true;
        }
        public void Save() {
            new MockEntityDAO().Save(this);
        }

        public override bool Equals(object obj)
        {
            if (this.Id == 0)
                return Object.Equals(this, obj);
            else
            {
                MockEntity that = obj as MockEntity;
                if( that == null)
                    return false;
                return this.Id.Equals(that.Id);
            }    
                
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }

   
}