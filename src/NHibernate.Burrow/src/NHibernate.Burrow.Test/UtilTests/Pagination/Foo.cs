namespace NHibernate.Burrow.Test.Pagination
{
	public class Foo
	{
#pragma warning disable 649
		private int id;
#pragma warning restore 649
		public virtual int Id
		{
			get { return id; }
		}

		private string name;
		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string description;
		public virtual string Description
		{
			get { return description; }
			set { description = value; }
		}

		public Foo()
		{
		}

		public Foo(string name, string description)
		{
			this.name = name;
			this.description = description;
		}
	}
}