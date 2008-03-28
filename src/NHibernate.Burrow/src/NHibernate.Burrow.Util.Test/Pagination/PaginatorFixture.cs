using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Burrow.Util.GenericImpl;
using NHibernate.Burrow.Util.Pagination;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.Util.Test.Pagination
{
    [TestFixture]
    public class PaginatorFixture : TestCase
    {
        protected override IList Mappings
        {
            get { return new string[] {"Pagination.PagTest.hbm.xml"}; }
        }

        public const int totalFoo = 15;

        protected override void OnSetUp()
        {
            ISession s = OpenSession();
            for (int i = 0; i < totalFoo; i++)
            {
                Foo f = new Foo("N" + i, "D" + i);
                s.Save(f);
            }
            s.Flush();
        }

        protected override void OnTearDown()
        {
            LastOpenedSession.Delete("from Foo");
            LastOpenedSession.Flush();
            LastOpenedSession.Close();
        }

        [Test]
        public void AutoCalcPages()
        {
            Paginator<NoFoo> ptor =
                new Paginator<NoFoo>(3, new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All")), true);
            // check init state
            Assert.AreEqual(1, ptor.FirstPageNumber);
            Assert.AreEqual(5, ptor.LastPageNumber);
            Assert.AreEqual(1, ptor.CurrentPageNumber);
            Assert.IsTrue(ptor.HasNext);
            Assert.IsFalse(ptor.HasPrevious);
            IList<NoFoo> lpage = ptor.GetLastPage();
            Assert.AreEqual(3, lpage.Count);
            Assert.AreEqual(5, ptor.CurrentPageNumber);
            Assert.IsFalse(ptor.HasNext);
            Assert.IsTrue(ptor.HasPrevious);

            // Partial last page
            ptor =
                new Paginator<NoFoo>(10, new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All")),
                                     true);
            Assert.AreEqual(2, ptor.LastPageNumber);
        }

        [Test]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void HasNotPages()
        {
            string hql = "from Foo where Name = '-N123'";
            PaginableRowsCounterQuery<Foo> pag =
                new PaginableRowsCounterQuery<Foo>(LastOpenedSession, new DetachedQuery(hql));
            Paginator<Foo> ptor = new Paginator<Foo>(3, pag);

            Assert.AreEqual(false, ptor.HasPages);
            Assert.AreEqual(false, ptor.HasPrevious);
            Assert.AreEqual(false, ptor.HasNext);
            Assert.AreEqual(0, ptor.RowsCount.Value);

            //throw ArgumentOutOfRangeException
            IList<Foo> lpage = ptor.GetPage(1);
        }

        [Test]
        public void HasPages()
        {
            string hql = "from Foo where Name = 'N1'";
            PaginableRowsCounterQuery<Foo> pag =
                new PaginableRowsCounterQuery<Foo>(LastOpenedSession, new DetachedQuery(hql));
            Paginator<Foo> ptor = new Paginator<Foo>(3, pag);

            Assert.AreEqual(true, ptor.HasPages);
            Assert.AreEqual(1, ptor.RowsCount);
            Assert.AreEqual(false, ptor.HasPrevious);
            Assert.AreEqual(false, ptor.HasNext);

            IList<Foo> lpage = ptor.GetPage(1);
            Assert.AreEqual(1, lpage.Count);
        }

        [Test]
        public void PageMoving()
        {
            Paginator<NoFoo> ptor =
                new Paginator<NoFoo>(3, new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All")), true);
            IList<NoFoo> entities = ptor.GetFirstPage();
            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual(1, ptor.CurrentPageNumber);

            entities = ptor.GetNextPage();
            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual(2, ptor.CurrentPageNumber);
            Assert.AreEqual("N3", entities[0].Name);
            Assert.AreEqual("N4", entities[1].Name);
            Assert.AreEqual("N5", entities[2].Name);

            entities = ptor.GetPage(4);
            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual(4, ptor.CurrentPageNumber);
            Assert.AreEqual("N9", entities[0].Name);
            Assert.AreEqual("N10", entities[1].Name);
            Assert.AreEqual("N11", entities[2].Name);

            entities = ptor.GetPreviousPage();
            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual(3, ptor.CurrentPageNumber);
            Assert.AreEqual("N6", entities[0].Name);
            Assert.AreEqual("N7", entities[1].Name);
            Assert.AreEqual("N8", entities[2].Name);
        }

        [Test]
        public void SimplePaginator()
        {
            // with "AutoCalcPages mode" DISABLED
            Paginator<NoFoo> ptor =
                new Paginator<NoFoo>(3, new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All")));
            // check init state
            Assert.AreEqual(1, ptor.FirstPageNumber);
            Assert.IsFalse(ptor.LastPageNumber.HasValue);
            Assert.IsFalse(ptor.CurrentPageNumber.HasValue);
            Assert.IsTrue(ptor.HasNext);
            Assert.IsFalse(ptor.HasPrevious);
            IList<NoFoo> lpage = ptor.GetFirstPage();
            Assert.AreEqual(3, lpage.Count);
            Assert.AreEqual(1, ptor.CurrentPageNumber);
            // check lastPage sill unavailable
            Assert.IsFalse(ptor.LastPageNumber.HasValue);
        }

        [Test]
        public void WithCounter()
        {
            Paginator<NoFoo> ptor =
                new Paginator<NoFoo>(5, new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All")),
                                     new NamedQueryRowsCounter("NoFoo.Count.All"));
            Assert.AreEqual(3, ptor.LastPageNumber);
        }

        [Test]
        public void WithSelfCounter()
        {
            DetachedQuery dq = new DetachedQuery("from Foo f where f.Name like :p1");
            dq.SetString("p1", "%1_");
            IPaginable<Foo> fp = new PaginableRowsCounterQuery<Foo>(LastOpenedSession, dq);

            Paginator<Foo> ptor = new Paginator<Foo>(2, fp);
            Assert.IsTrue(ptor.AutoCalcPages);
            Assert.AreEqual(3, ptor.LastPageNumber);
            // check page 2
            IList<Foo> lpage = ptor.GetPage(2);
            Assert.AreEqual(2, lpage.Count);
            Assert.AreEqual(2, ptor.CurrentPageNumber);
            Assert.AreEqual("N12", lpage[0].Name);
            Assert.AreEqual("N13", lpage[1].Name);
        }
    }
}