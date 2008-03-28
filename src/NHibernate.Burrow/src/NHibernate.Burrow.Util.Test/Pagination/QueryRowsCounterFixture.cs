using System.Collections;
using NHibernate.Burrow.Util.Pagination;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.Util.Test.Pagination
{
    [TestFixture]
    public class QueryRowsCounterFixture : TestCase
    {
        protected override IList Mappings
        {
            get { return new string[] {"Pagination.PagTest.hbm.xml"}; }
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
            IRowsCounter rc = new QueryRowsCounter("select count(*) from Foo");
            using (ISession s = OpenSession())
            {
                Assert.AreEqual(totalFoo, rc.GetRowsCount(s));
            }
        }

        [Test]
        public void RowsCountTransforming()
        {
            DetachedQuery dq = new DetachedQuery("from Foo f where f.Name like :p1");
            dq.SetString("p1", "%1_");
            IRowsCounter rc = QueryRowsCounter.Transforming(dq);
            using (ISession s = OpenSession())
            {
                Assert.AreEqual(5, rc.GetRowsCount(s));
            }
        }

        [Test]
        public void RowsCountUsingParameters()
        {
            IDetachedQuery dq =
                new DetachedQuery("select count(*) from Foo f where f.Name like :p1").SetString("p1", "%1_");
            IRowsCounter rc = new QueryRowsCounter(dq);
            using (ISession s = OpenSession())
            {
                Assert.AreEqual(5, rc.GetRowsCount(s));
            }
        }
    }
}