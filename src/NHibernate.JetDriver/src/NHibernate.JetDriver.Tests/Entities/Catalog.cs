namespace NHibernate.JetDriver.Tests.Entities
{
    public class Catalog
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual Category Category
        {
            get;
            set;
        }

        public virtual ProductType ProductType
        {
            get;
            set;
        }
    }
}