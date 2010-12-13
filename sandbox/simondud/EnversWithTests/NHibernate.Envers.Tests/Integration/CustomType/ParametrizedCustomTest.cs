using System.Collections.Generic;
using NHibernate.Envers.Tests.Entities.CustomType;
using NUnit.Framework;

namespace NHibernate.Envers.Tests.Integration.CustomType
{
	[TestFixture, Ignore("Waiting with this one. Need to find a way to get the user type in runtime")]
	public class ParametrizedCustomTest : TestBase
	{
		private int pcte_id;

		protected override IEnumerable<string> Mappings
		{
			get { return new[] { "Entities.CustomType.Mapping.hbm.xml" }; }
		}

		[SetUp]
		public void Setup()
		{
			var pcte = new ParametrizedCustomTypeEntity();

			using(var tx = Session.BeginTransaction())
			{
				pcte.Str = "U";
				Session.Save(pcte);
				tx.Commit();
			}
			using(var tx = Session.BeginTransaction())
			{
				pcte.Str = "V";
				tx.Commit();
			}
			pcte_id = pcte.Id;
		}

		[Test]
		public void VerifyRevisionCount()
		{
			CollectionAssert.AreEquivalent(new[] { 1, 2 }, AuditReader.GetRevisions(typeof(ParametrizedCustomTypeEntity), pcte_id));
		}

		[Test]
		public void VerifyHistoryOfPcte()
		{
			var rev1 = AuditReader.Find<ParametrizedCustomTypeEntity>(pcte_id, 1);
			var rev2 = AuditReader.Find<ParametrizedCustomTypeEntity>(pcte_id, 2);

			Assert.AreEqual(rev1.Str, "xUy");
			Assert.AreEqual(rev2.Str, "xVy");
		}
	}
}