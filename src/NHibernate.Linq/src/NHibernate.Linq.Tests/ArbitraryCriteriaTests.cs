using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class ArbitraryCriteriaTests : BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		public void can_create_linq_query_from_arbitrary_criteria_query()
		{
			var criteria = session.CreateCriteria<User>();
			criteria.Add(Restrictions.Le("RegisteredAt", new DateTime(2000, 1, 1)));

			var query = session.Linq<User>(criteria)
				.Where(u => u.Name == "nhibernate" || u.Name == "ayende");

			var list = query.ToList();
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual("nhibernate", list.Single().Name);
		}

		[Test]
		public void can_reuse_query_after_creating_from_criteria()
		{
			var criteria = session.CreateCriteria<User>();
			var query = session.Linq<User>(criteria);

			int totalCount = query.Count();
			Assert.AreEqual(3, totalCount);

			var users = query.ToList();
			Assert.AreEqual(3, users.Count);
		}
	}
}