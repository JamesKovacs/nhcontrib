using System.Linq;

namespace NHibernate.Linq.Tests.Entities
{
	public class TestContext : NHibernateContext
	{
		public TestContext(ISession session) : base(session)
		{
		}

		public IOrderedQueryable<User> Users
		{
			get { return Session.Linq<User>(); }
		}
	}
}