using Iesi.Collections.Generic;

namespace NHibernate.JetDriver.Tests.Entities
{
    public class Category
    {
        public Category()
        {
            Catalogs = new HashedSet<Catalog>();
        }

        public virtual int Id
        {
            get;
            set;
        }

        public virtual ISet<Catalog> Catalogs
        {
            get; 
            private set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual Product Product
        {
            get;
            set;
        }
    }
}