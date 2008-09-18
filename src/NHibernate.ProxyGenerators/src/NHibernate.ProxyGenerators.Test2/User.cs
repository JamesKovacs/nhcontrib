namespace NHibernate.ProxyGenerators.Test2
{
	public class User
	{
		private int _id;

		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}
	}
}