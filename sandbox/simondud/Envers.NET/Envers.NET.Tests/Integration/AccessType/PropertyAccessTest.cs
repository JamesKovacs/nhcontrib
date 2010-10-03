using System.Collections.Generic;
using NUnit.Framework;

namespace Envers.NET.Tests.NH3.Integration.AccessType
{
    public class PropertyAccessTest : TestBase
    {
        private int id;

        protected override IEnumerable<string> Mappings
        {
            get { return new[] { "Integration.AccessType.Mapping.hbm.xml" }; }
        }

        [SetUp]
        public void Setup()
        {
            var fa = new PropertyAccessEntity { Data = "first" };
            using (var tx = Session.BeginTransaction())
            {
                id = (int)Session.Save(fa);
                tx.Commit();
            }
            using (var tx = Session.BeginTransaction())
            {
                fa.Data = "second";
                tx.Commit();
            }
        }

        [Test]
        public void VerifyRevisionCount()
        {
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, AuditReader.GetRevisions(typeof(PropertyAccessEntity), id));
        }

        [Test]
        public void VerifyHistory()
        {
            Assert.AreEqual("first", AuditReader.Find<PropertyAccessEntity>(typeof(PropertyAccessEntity), id, 1).Data);
            Assert.AreEqual("second", AuditReader.Find<PropertyAccessEntity>(typeof(PropertyAccessEntity), id, 2).Data);
        }

    }
}