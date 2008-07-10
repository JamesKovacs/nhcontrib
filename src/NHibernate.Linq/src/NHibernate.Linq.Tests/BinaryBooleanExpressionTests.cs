using System;
using NUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class BinaryBooleanExpressionTests : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        [Test]
        public void TimesheetsWithEqualsTrue()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where timesheet.Entries.Any() == true
                         select timesheet).ToList();

            Assert.AreEqual(2, query.Count);
        }

        [Test]
        public void NegativeTimesheetsWithEqualsTrue()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where !timesheet.Entries.Any() == true
                         select timesheet).ToList();

            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void TimesheetsWithEqualsFalse()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where timesheet.Entries.Any() == false
                         select timesheet).ToList();

            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void NegativeTimesheetsWithEqualsFalse()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where !timesheet.Entries.Any() == false
                         select timesheet).ToList();

            Assert.AreEqual(2, query.Count);
        }
        /******************************************************************************/
        [Test]
        public void TimesheetsWithNotEqualsTrue()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where timesheet.Entries.Any() != true
                         select timesheet).ToList();

            Assert.AreEqual(1, query.Count);
        }

        [Test]
        public void NegativeTimesheetsWithNotEqualsTrue()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where !timesheet.Entries.Any() != true
                         select timesheet).ToList();

            Assert.AreEqual(2, query.Count);
        }

        [Test]
        public void TimesheetsWithNotEqualsFalse()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where timesheet.Entries.Any() != false
                         select timesheet).ToList();

            Assert.AreEqual(2, query.Count);
        }

        [Test]
        public void NegativeTimesheetsWithNotEqualsFalse()
        {
            var query = (from timesheet in session.Linq<Timesheet>()
                         where !timesheet.Entries.Any() != false
                         select timesheet).ToList();

            Assert.AreEqual(1, query.Count);
        }
    }
}
