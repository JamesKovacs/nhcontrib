using System.Linq;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class ProxiedEntityTests : BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		public void QueryForProxiedEntity()
		{
			var query = (from e in session.Linq<ProxiedEntity>()
						 where e.Id == 1
						 select e);

			ObjectDumper.Write(query);
		}

		[Test]
		public void AnyQueryForMemberOfProxiedEntity()
		{
			var ent = new ProxiedEntity()
						{
							Child = new ProxiedEntityChild()
							{
								Children = new List<AnotherChild>()
								{
									new AnotherChild()
										{
											Foo = "foo1",
										}
								}
							}

						};

			session.Save(ent);

			session.Flush();

			var query = from p in session.Linq<ProxiedEntity>()
						where p.Child.Children.Any(x => x.Foo == "foo1")
						select p;

			var result = query.ToList();
			Assert.That(result.Count, Is.EqualTo(1));
		}

		[Test]
		[Ignore("Query on collections of child entities does not work correctly")]
		public void ContainsQueryForMemberOfProxiedEntity()
		{
			var ent = new ProxiedEntity()
			{
				Child = new ProxiedEntityChild()
				{
					Children = new List<AnotherChild>()
								{
									new AnotherChild()
										{
											Foo = "foo1",
										}
								}
				}

			};

			session.Save(ent);

			session.Flush();

			var child = session.CreateQuery("from AnotherChild c where c.Foo = 'foo1'").UniqueResult<AnotherChild>();

			var query = from p in session.Linq<ProxiedEntity>()
						where p.Child.Children.Contains(child)
						select p;

			var result = query.ToList();
			Assert.That(result.Count, Is.EqualTo(1));
		}
	}
}