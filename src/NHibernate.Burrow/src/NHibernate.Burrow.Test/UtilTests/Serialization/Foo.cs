using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Test.Serialization
{
    [Serializable]
    public class Foo
    {
        private int id;
        private string name;
        private decimal price;
        private string description;
        [NonSerialized]
        public string nonSerializablefield;

        private IList<FooChild> children;


        public Foo()
        {
            children = new List<FooChild>();      
        }


        public Foo(int id, string name, decimal price, string description)
            : this(id, name, description)
        {
            this.price = price;
        }

        public Foo(int id, string name, string description) :this()
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

        public IList<FooChild> Children
        {
            get { return children; }
            set { children = value; }
        }
    }

}
