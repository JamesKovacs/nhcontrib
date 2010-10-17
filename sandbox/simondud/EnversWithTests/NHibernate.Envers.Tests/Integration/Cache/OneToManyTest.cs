using System.Collections.Generic;
using NHibernate.Envers.Tests.Entities.OneToMany;
using NUnit.Framework;

namespace NHibernate.Envers.Tests.Integration.Cache
{
    [TestFixture, Explicit("Not green yet - also missing test in this fixture")]
    public class OneToManyTest : TestBase
    {
        private int ed1_id;
        private int ed2_id;

        private int ing1_id;
        private int ing2_id;

        protected override IEnumerable<string> Mappings
        {
            get { return new[] { "Entities.OneToMany.Mapping.hbm.xml" }; }
        }

        [SetUp]
        public void Setup()
        {
            var ed1 = new SetRefEdEntity { Id = 1, Data = "data_ed_1" };
            var ed2 = new SetRefEdEntity { Id = 2, Data = "data_ed_2" };

            var ing1 = new SetRefIngEntity { Id = 1, Data = "data_ing_1" };
            var ing2 = new SetRefIngEntity { Id = 2, Data = "data_ing_2" };

            using (var tx = Session.BeginTransaction()) //rev1
            {
                Session.Save(ed1);
                Session.Save(ed2);
                ing1.Reference = ed1;
                ing2.Reference = ed1;
                Session.Save(ing1);
                Session.Save(ing2);
                tx.Commit();
            }
            using (var tx = Session.BeginTransaction()) //rev2
            {
                ing1.Reference = ed2;
                ing2.Reference = ed2;
                tx.Commit();
            }
            ed1_id = ed1.Id;
            ed2_id = ed2.Id;
            ing1_id = ing1.Id;
            ing2_id = ing2.Id;
        }

        [Test]
        public void VerifyCacheReferenceAfterFind()
        {
            var ed1_rev1 = AuditReader.Find<SetRefEdEntity>(ed1_id, 1);
            var ing1_rev1 = AuditReader.Find<SetRefIngEntity>(ing1_id, 1);
            var ing2_rev1 = AuditReader.Find<SetRefIngEntity>(ing2_id, 1);

            Assert.AreSame(ing1_rev1.Reference, ed1_rev1);
            Assert.AreSame(ing2_rev1.Reference, ed1_rev1);

        }
    }
}