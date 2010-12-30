using System.Collections.Generic;
using NHibernate.Envers.Tests.Entities.ManyToMany;
using NUnit.Framework;

namespace NHibernate.Envers.Tests.Integration.ManyToMany
{
	[TestFixture, Ignore("Continue with this later")]
	public class BasicListTest : TestBase
	{
		private int ed1_id;
		private int ed2_id;
		private int ing1_id;
		private int ing2_id;

		protected override void Initialize()
		{
			ed1_id = 123;
			ed2_id = 444;
			ing1_id = 3;
			ing2_id = 894;

			var ed1 = new ListOwnedEntity {Id = ed1_id, Data = "data_ed_1"};
			var ed2 = new ListOwnedEntity {Id = ed2_id, Data = "data_ed_2"};
			var ing1 = new ListOwningEntity {Id = ing1_id, Data = "data_ing_1"};
			var ing2 = new ListOwningEntity {Id = ing2_id, Data = "data_ing_2"};

			using (var tx = Session.BeginTransaction())
			{
				Session.Save(ed1);
				Session.Save(ed2);
				Session.Save(ing1);
				Session.Save(ing2);
				tx.Commit();
			}
			using (var tx = Session.BeginTransaction())
			{
				ing1 = Session.Get<ListOwningEntity>(ing1_id);
				ing2 = Session.Get<ListOwningEntity>(ing2_id);
				ed1 = Session.Get<ListOwnedEntity>(ed1_id);
				ed2 = Session.Get<ListOwnedEntity>(ed2_id);

				ing1.References = new List<ListOwnedEntity> {ed1};
				ing2.References = new List<ListOwnedEntity> {ed1, ed2};
				tx.Commit();
			}
			using (var tx = Session.BeginTransaction())
			{
				ing1 = Session.Get<ListOwningEntity>(ing1_id);
				ed2 = Session.Get<ListOwnedEntity>(ed2_id);
				ed1 = Session.Get<ListOwnedEntity>(ed1_id);
				
				ing1.References.Add(ed2);
				tx.Commit();
			}
			using (var tx = Session.BeginTransaction())
			{
				ing1 = Session.Get<ListOwningEntity>(ing1_id);
				ed2 = Session.Get<ListOwnedEntity>(ed2_id);
				ed1 = Session.Get<ListOwnedEntity>(ed1_id);

				ing1.References.Remove(ed1);
				tx.Commit();
			}
			using (var tx = Session.BeginTransaction())
			{
				ing1 = Session.Get<ListOwningEntity>(ing1_id);

				ing1.References = null;
				tx.Commit();
			}
		}

		[Test]
		public void VerifyRevisionCount()
		{
			CollectionAssert.AreEquivalent(new[] { 1, 2, 4 }, AuditReader.GetRevisions(typeof(ListOwnedEntity), ed1_id));
			CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 5 }, AuditReader.GetRevisions(typeof(ListOwnedEntity), ed2_id));
			CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4, 5 }, AuditReader.GetRevisions(typeof(ListOwningEntity), ing1_id));
			CollectionAssert.AreEquivalent(new[] { 1, 2 }, AuditReader.GetRevisions(typeof(ListOwningEntity), ing2_id));
		}



		protected override IEnumerable<string> Mappings
		{
			get
			{
				return new[]{"Entities.ManyToMany.Mapping.hbm.xml"};
			}
		}
	}
}