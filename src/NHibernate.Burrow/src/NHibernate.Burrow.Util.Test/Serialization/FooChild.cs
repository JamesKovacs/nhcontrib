using System;

namespace NHibernate.Burrow.Util.Test.Serialization
{
    [Serializable]
    public class FooChild
    {
        private int id;

        public FooChild(int id)
        {
            this.id = id;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}