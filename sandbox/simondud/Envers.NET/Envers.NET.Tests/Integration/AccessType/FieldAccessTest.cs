using System.Collections.Generic;
using NUnit.Framework;

namespace Envers.NET.Tests.Integration.AccessType
{
    [TestFixture]
    public class FieldAccessTest : TestBase
    {
        private int id;

        protected override IEnumerable<string> Mappings
        {
            get { return new[] {"Integration.AccessType.Mapping.hbm.xml"}; }
        }

        [SetUp]
        public void Setup()
        {
            var fa = new FieldAccessEntity {Data = "first"};
            using(var tx = Session.BeginTransaction())
            {
                id = (int)Session.Save(fa);
                tx.Commit();
            }
            using(var tx = Session.BeginTransaction())
            {
                fa.Data = "second";
                tx.Commit();
            }
        }
        
        [Test]
        public void VerifyRevisionCount()
        {
            CollectionAssert.AreEquivalent(new[] {1, 2}, AuditReader.GetRevisions(typeof (FieldAccessEntity), id));
        }

        [Test]
        public void VerifyHistory()
        {
            Assert.AreEqual("first", AuditReader.Find<FieldAccessEntity>(typeof (FieldAccessEntity), id, 1));
            Assert.AreEqual("second", AuditReader.Find<FieldAccessEntity>(typeof (FieldAccessEntity), id, 2));
        }
    }
}