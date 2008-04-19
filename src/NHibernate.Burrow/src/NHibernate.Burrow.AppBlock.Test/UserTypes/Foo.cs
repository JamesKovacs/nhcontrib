using System;

namespace NHibernate.Burrow.AppBlock.Test.UserTypes
{
    [Serializable]
    public class Foo
    {
        private string description;
        private int id;
        private string name;
        private decimal price;

        public Foo() {}

        public Foo(int id, string name, decimal price, string description) : this(id, name, description)
        {
            this.price = price;
        }

        public Foo(int id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}