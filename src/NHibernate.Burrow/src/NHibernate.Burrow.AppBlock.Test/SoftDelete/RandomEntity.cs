using NHibernate.Burrow.AppBlock.SoftDelete;
using System;

namespace NHibernate.Burrow.AppBlock.Test.SoftDelete
{         
    public class RandomEntity
    {
        private int id;
        private string name;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
