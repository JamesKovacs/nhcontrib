namespace NHibernate.Burrow.Util.Test.Pagination
{
    public class Foo
    {
#pragma warning disable 649
        private string description;
        private int id;
#pragma warning restore 649

        private string name;

        public Foo() {}

        public Foo(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public virtual int Id
        {
            get { return id; }
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}