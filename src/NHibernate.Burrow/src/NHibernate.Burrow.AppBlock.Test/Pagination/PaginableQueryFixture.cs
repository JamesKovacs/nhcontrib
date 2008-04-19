using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Burrow.AppBlock.GenericImpl;
using NHibernate.Burrow.AppBlock.Pagination;
using NHibernate.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.Pagination
{
    [TestFixture]
    public class PaginableQueryFixture : TestCase
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
        public void GetPageTest()
        {
            // Page count start from 1
            using (OpenSession())
            {
                IPaginable<NoFoo> pg = new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All"));
                IList<NoFoo> l = pg.GetPage(3, 2);
                Assert.AreEqual(3, l.Count);
                Assert.AreEqual("N3", l[0].Name);
                Assert.AreEqual("N4", l[1].Name);
                Assert.AreEqual("N5", l[2].Name);

                l = pg.GetPage(2, 1);
                Assert.AreEqual(2, l.Count);
                Assert.AreEqual("N0", l[0].Name);
                Assert.AreEqual("N1", l[1].Name);

                // If pageSize=10 the page 2 have 5 elements
                l = pg.GetPage(10, 2);
                Assert.AreEqual(5, l.Count);
            }

            // Add an element from other session
            Foo fAdded;
            using (ISession s = OpenSession())
            {
                fAdded = new Foo("NZ", "DZ");
                s.Save(fAdded);
                s.Flush();
            }

            // Reload the same page and have the new element
            using (OpenSession())
            {
                IPaginable<NoFoo> pg = new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All"));
                IList<NoFoo> l = pg.GetPage(10, 2);
                // If pageSize=10 the page 2 have 6 elements
                Assert.AreEqual(6, l.Count);
            }

            // clean up
            using (ISession s = OpenSession())
            {
                s.Delete(fAdded);
                s.Flush();
            }
        }

        [Test]
        public void ListAllTest()
        {
            using (OpenSession())
            {
                IPaginable<NoFoo> pg = new NoFooPaginable(LastOpenedSession, new DetachedNamedQuery("NoFoo.All"));
                IList<NoFoo> l = pg.ListAll();
                Assert.AreEqual(15, l.Count);
            }

            using (OpenSession())
            {
                IPaginable<Foo> pf = new PaginableQuery<Foo>(LastOpenedSession, new DetachedQuery("from Foo"));
                IList<Foo> l = pf.ListAll();
                Assert.AreEqual(15, l.Count);
            }
        }

        [Test]
        public void NullArgumentNotAllowed()
        {
            try
            {
                new PaginableQuery<Foo>(null, new DetachedQuery("from Foo"));
                Assert.Fail("No exception for null argument.");
            }
            catch (ArgumentNullException) {}
            try
            {
                using (OpenSession())
                {
                    new PaginableQuery<Foo>(LastOpenedSession, null);
                    Assert.Fail("No exception for null argument.");
                }
            }
            catch (ArgumentNullException) {}
        }

        [Test]
        public void PaginableRowsCount()
        {
            DetachedQuery dq = new DetachedQuery("from Foo f where f.Name like :p1");
            dq.SetString("p1", "N_");
            using (ISession s = OpenSession())
            {
                IPaginable<Foo> fp = new PaginableRowsCounterQuery<Foo>(LastOpenedSession, dq);
                IList<Foo> l = fp.GetPage(5, 1);
                Assert.AreEqual(5, l.Count);
                Assert.AreEqual(10, ((IRowsCounter) fp).GetRowsCount(s));
            }
        }
    }
}