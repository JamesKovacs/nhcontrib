using System.Collections;
using NHibernate;
using NUnit.Framework;
using NHibernate.Burrow.Util.NH.Impl;
using NHibernate.Burrow.Util.Pagination;

namespace NHibernate.Burrow.Test.Pagination
{
	[TestFixture]
	public class NamedQueryRowsCounterFixture:TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] { "Pagination.PagTest.hbm.xml" }; }
		}

		public const int totalFoo = 15;
		protected override void OnSetUp()
		{
			using (ISession s = OpenSession())
			{
				for (int i = 0; i < totalFoo; i++)
				{
					Foo f = new Foo("N" + i, "D" + i);
					s.Save(f);
				}
				s.Flush();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Foo");
				s.Flush();
			}
		}

		[Test]
		public void RowsCount()
		{
			IRowsCounter rc = new NamedQueryRowsCounter("Foo.Count.All");
			using (ISession s = OpenSession())
			{
				Assert.AreEqual(totalFoo, rc.GetRowsCount(s));
			}
		}

		[Test]
		public void RowsCountUsingParameters()
		{
			DetachedNamedQuery q = new DetachedNamedQuery("Foo.Count.Parameters");
			q.SetString("p1", "%1_");
			IRowsCounter rc = new NamedQueryRowsCounter(q);
			using (ISession s = OpenSession())
			{
				Assert.AreEqual(5, rc.GetRowsCount(s));
			}
		}

	}
}
