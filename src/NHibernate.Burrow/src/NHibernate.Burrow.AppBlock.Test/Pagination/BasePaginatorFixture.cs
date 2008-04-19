using System;
using NHibernate.Burrow.AppBlock.Pagination;
using NUnit.Framework;

namespace NHibernate.Burrow.AppBlock.Test.Pagination
{
    [TestFixture]
    public class BasePaginatorFixture
    {
        private class GPPaginatorCrack : BasePaginator
        {
            public GPPaginatorCrack() {}

            public GPPaginatorCrack(int lastPageNumber) : base(lastPageNumber) {}

            public new void GotoPageNumber(int pageNumber)
            {
                base.GotoPageNumber(pageNumber);
            }
        }

        [Test, ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void GotoOutOfPages()
        {
            GPPaginatorCrack pg = new GPPaginatorCrack(10);
            pg.GotoPageNumber(11);
        }

        [Test]
        public void KnowLastPage()
        {
            GPPaginatorCrack pg = new GPPaginatorCrack(100);
            Assert.AreEqual(100, pg.LastPageNumber);
            Assert.AreEqual(1, pg.CurrentPageNumber);
            Assert.AreEqual(1, pg.FirstPageNumber);
            Assert.AreEqual(2, pg.NextPageNumber);
            Assert.AreEqual(1, pg.PreviousPageNumber);
            Assert.IsFalse(pg.HasPrevious);
            Assert.IsTrue(pg.HasNext);

            pg.GotoPageNumber(10);
            Assert.AreEqual(10, pg.CurrentPageNumber);
            Assert.AreEqual(100, pg.LastPageNumber);
            Assert.AreEqual(1, pg.FirstPageNumber);
            Assert.AreEqual(11, pg.NextPageNumber);
            Assert.AreEqual(9, pg.PreviousPageNumber);
            Assert.IsTrue(pg.HasPrevious);
            Assert.IsTrue(pg.HasNext);

            pg.GotoPageNumber(100);
            Assert.IsFalse(pg.HasNext);
        }

        [Test]
        public void NoPageAvailable()
        {
            GPPaginatorCrack pg = new GPPaginatorCrack(0);
            Assert.AreEqual(0, pg.LastPageNumber);
            Assert.AreEqual(0, pg.CurrentPageNumber);
            Assert.AreEqual(0, pg.NextPageNumber);
            Assert.AreEqual(0, pg.PreviousPageNumber);
            Assert.AreEqual(0, pg.FirstPageNumber);
            Assert.IsFalse(pg.HasPrevious);
            Assert.IsFalse(pg.HasNext);
        }

        [Test]
        public void UnknowLastPage()
        {
            GPPaginatorCrack pg = new GPPaginatorCrack();
            Assert.IsFalse(pg.CurrentPageNumber.HasValue);
            Assert.IsFalse(pg.LastPageNumber.HasValue);
            Assert.AreEqual(1, pg.FirstPageNumber);
            Assert.AreEqual(1, pg.NextPageNumber);
            Assert.AreEqual(1, pg.PreviousPageNumber);
            Assert.IsFalse(pg.HasPrevious);
            Assert.IsTrue(pg.HasNext);

            pg.GotoPageNumber(1000);
            Assert.AreEqual(1000, pg.CurrentPageNumber);
            Assert.IsFalse(pg.LastPageNumber.HasValue);
            Assert.AreEqual(1, pg.FirstPageNumber);
            Assert.AreEqual(1001, pg.NextPageNumber);
            Assert.AreEqual(999, pg.PreviousPageNumber);
            Assert.IsTrue(pg.HasPrevious);
            Assert.IsTrue(pg.HasNext);
        }
    }
}