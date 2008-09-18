namespace NHibernate.ProxyGenerators.Test
{
	public class Person
	{
		private int _id;

		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}
	}
}
