using System;

namespace NHibernate.JetDriver.Tests.Entities
{
    public class Product
    {
        public virtual object Id
        {
            get;
            set;
        }

        public virtual String Name
        {
            get;
            set;
        }
    }
}