using System.Collections.Generic;
using NHibernate.Envers.Exceptions;
using NUnit.Framework;

namespace Envers.NET.Tests.Integration.Basic
{
    [TestFixture]
    public class NonVersionedTest : TestBase
    {
        private int id1;

        protected override IEnumerable<string> Mappings
        {
            get { return new[] { "Integration.Basic.Mapping.hbm.xml" }; }
        }

        [SetUp]
        public void Setup()
        {
            var bte1 = new BasicTestEntity3 { Str1 = "x", Str2 = "y" };
            using (var tx = Session.BeginTransaction())
            {
                id1 = (int)Session.Save(bte1);
                tx.Commit();
            }
            using (var tx = Session.BeginTransaction())
            {
                bte1.Str1 = "a";
                bte1.Str2 = "b";
                tx.Commit();
            }
        }

        [Test]
        public void VerifyRevisionCount()
        {
            Assert.Throws<NotAuditedException>(() =>
                AuditReader.GetRevisions(typeof (BasicTestEntity3), id1)
            );
        }

        [Test]
        public void VerifyHistory()
        {
            Assert.Throws<NotAuditedException>(() =>
                AuditReader.Find<BasicTestEntity3>(id1, 1)
            );
        }
    }
}