using System.Collections.Generic;

namespace NHibernate.Burrow.Test.MultiDB
{
    public class MockEntity2
    {
        private int id;

        private string name;
        private int number;

        private int preDeletedPerformed = 0;
        private IList<string> stringList = new List<string>();

       
        public MockEntity2()
        {
            Name = string.Empty;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public IList<string> StringList
        {
            get { return stringList; }
            set { stringList = value; }
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

        public bool Delete()
        {
            preDeletedPerformed++;
            new MockEntity2DAO().Delete(this);
            return true;
        }

        public void Save()
        {
            new MockEntity2DAO().Save(this);
        }

        public override bool Equals(object obj)
        {
            if (Id == 0)
            {
                return Equals(this, obj);
            }
            else
            {
                MockEntity2 that = obj as MockEntity2;
                if (that == null)
                {
                    return false;
                }
                return Id.Equals(that.Id);
            }
        }

        public override int GetHashCode()
        {
            if (Id == 0)
                return base.GetHashCode();
            else return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}