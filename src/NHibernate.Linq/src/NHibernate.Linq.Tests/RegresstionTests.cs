using NUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Linq;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class RegresstionTests : BaseTest
	{
		protected override string ConnectionStringName
		{
			get
			{
				return "Test";
			}
		}

		/// <summary>
		/// http://aspzone.com/tech/nhibernate-linq-troubles/
		/// </summary>
		[Test]
		public void HierarchicalQueries()
		{
			var children = from s in session.Linq<Role>()
			            where s.ParentRole != null
			            select s;
			Assert.AreEqual(0, children.Count());

			var roots = from s in session.Linq<Role>()
						   where s.ParentRole == null
						   select s;
			Assert.AreEqual(2, roots.Count());
		}
	}
}